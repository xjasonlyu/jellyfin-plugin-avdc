using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AVDC.Configuration;
using MediaBrowser.Common.Json;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC
{
    public sealed class ApiClient
    {
        private const string Actress = "/actress/";
        private const string Metadata = "/metadata/";
        private const string ActressImage = "/image/actress/";
        private const string PrimaryImage = "/image/primary/";
        private const string BackdropImage = "/image/backdrop/";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger _logger;


        public ApiClient(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions(JsonDefaults.GetOptions());
        }

        private static PluginConfiguration Config => Plugin.Instance?.Configuration ?? new PluginConfiguration();

        public static string GetActressUrl(string name)
        {
            return $"{Config.Server}{Actress}{name}";
        }

        public static string GetMetadataUrl(string vid)
        {
            return $"{Config.Server}{Metadata}{vid}";
        }

        public static string GetActressImageUrl(string name)
        {
            return $"{Config.Server}{ActressImage}{name}";
        }

        public static string GetPrimaryImageUrl(string vid)
        {
            return $"{Config.Server}{PrimaryImage}{vid}";
        }

        public static string GetBackdropImageUrl(string vid)
        {
            return $"{Config.Server}{BackdropImage}{vid}";
        }

        /// <summary>
        ///     Get Video Metadata by Vid.
        /// </summary>
        /// <param name="vid">Video ID.</param>
        /// <param name="cancellationToken">Used to cancel the request.</param>
        /// <returns>Video Metadata.</returns>
        public async Task<Metadata> GetMetadata(string vid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(vid))
            {
                _logger.LogError("[AVDC] invalid vid: \"{Vid}\"", vid);
                return new Metadata();
            }

            try
            {
                var contentStream = await GetStream(GetMetadataUrl(vid), cancellationToken);
                return await JsonSerializer.DeserializeAsync<Metadata>(contentStream, _jsonOptions, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError("[AVDC] GetMetadata for {Vid} failed: {Message}", vid, e.Message);
                return new Metadata();
            }
        }

        /// <summary>
        ///     Get Actress Info by Name.
        /// </summary>
        /// <param name="name">Actress Name.</param>
        /// <param name="cancellationToken">Used to cancel the request.</param>
        /// <returns>Actress Info.</returns>
        public async Task<Actress> GetActress(string name, CancellationToken cancellationToken)
        {
            try
            {
                var contentStream = await GetStream(GetActressUrl(name), cancellationToken);
                return await JsonSerializer.DeserializeAsync<Actress>(contentStream, _jsonOptions, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError("[AVDC] GetActress for {Name} failed: {Message}", name, e.Message);
                return new Actress();
            }
        }

        /// <summary>
        ///     Get the response by HTTP without any other options.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="cancellationToken">Used to cancel the request.</param>
        /// <returns>Http Response.</returns>
        public async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken)
        {
            _logger.LogDebug("[AVDC] HTTP request to: {Url}", url);

            cancellationToken.ThrowIfCancellationRequested();

            var httpClient = _httpClientFactory.CreateClient();

            // Add Auth Token Header
            if (!string.IsNullOrEmpty(Config.Token) && url.StartsWith(Config.Server))
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Config.Token);

            var response = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<Stream> GetStream(string url, CancellationToken cancellationToken)
        {
            var response = await GetAsync(url, cancellationToken);
            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }
    }
}