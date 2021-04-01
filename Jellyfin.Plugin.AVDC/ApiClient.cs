using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AVDC.Configuration;
using Jellyfin.Plugin.AVDC.Models;
using MediaBrowser.Model.Serialization;
#if __EMBY__
using System.Net;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

#else
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.AVDC
{
    public sealed class ApiClient
    {
        private const string Actress = "/actress/";
        private const string Metadata = "/metadata/";
        private const string ActressImage = "/image/actress/";
        private const string PrimaryImage = "/image/primary/";
        private const string BackdropImage = "/image/backdrop/";

#if __EMBY__
        private readonly IHttpClient _httpClient;
#else
        private readonly IHttpClientFactory _httpClientFactory;
#endif
        private readonly ILogger _logger;
        private readonly IJsonSerializer _jsonSerializer;

#if __EMBY__
        public ApiClient(IHttpClient httpClient, IJsonSerializer jsonSerializer, ILogger logger)
        {
            _httpClient = httpClient;
#else
        public ApiClient(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
#endif
            _logger = logger;
            _jsonSerializer = jsonSerializer;
        }

        private static PluginConfiguration Config => Plugin.Instance?.Configuration ?? new PluginConfiguration();

        private static string GetActressUrl(string name)
        {
            return $"{Config.Server}{Actress}{name}";
        }

        private static string GetMetadataUrl(string vid)
        {
            return $"{Config.Server}{Metadata}{vid}";
        }

        public static string GetActressImageUrl(string name)
        {
            return $"{Config.Server}{ActressImage}{name}";
        }

        public static string GetActressImageUrl(string name, int index)
        {
            return $"{Config.Server}{ActressImage}{name}/{index}";
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
#if __EMBY__
                _logger.Error("[AVDC] invalid vid: \"{0}\"", vid);
#else
                _logger.LogError("[AVDC] invalid vid: \"{Vid}\"", vid);
#endif
                return new Metadata();
            }

            try
            {
                var contentStream = await GetStream(GetMetadataUrl(vid), cancellationToken);
                return _jsonSerializer.DeserializeFromStream<Metadata>(contentStream);
            }
            catch (Exception e)
            {
#if __EMBY__
                _logger.Error("[AVDC] GetMetadata for {0} failed: {1}", vid, e.Message);
#else
                _logger.LogError("[AVDC] GetMetadata for {Vid} failed: {Message}", vid, e.Message);
#endif
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
                return _jsonSerializer.DeserializeFromStream<Actress>(contentStream);
            }
            catch (Exception e)
            {
#if __EMBY__
                _logger.Error("[AVDC] GetActress for {0} failed: {1}", name, e.Message);
#else
                _logger.LogError("[AVDC] GetActress for {Name} failed: {Message}", name, e.Message);
#endif
                return new Actress();
            }
        }


#if __EMBY__
        /// <summary>
        ///     Get the response by HTTP without any other options.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="cancellationToken">Used to cancel the request.</param>
        /// <returns>HttpResponseInfo.</returns>
        public async Task<HttpResponseInfo> GetAsync(string url, CancellationToken cancellationToken)
        {
            var options = new HttpRequestOptions
            {
                Url = url,
                CancellationToken = cancellationToken
            };

            // Add Auth Token Header
            if (!string.IsNullOrEmpty(Config.Token) && url.StartsWith(Config.Server))
                options.RequestHeaders.Add("Authorization", $"Bearer {Config.Token}");

            var response = await _httpClient.GetResponse(options);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Bad Status Code: {response.StatusCode}");

            return response;
        }
#else
        /// <summary>
        ///     Get the response by HTTP without any other options.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="cancellationToken">Used to cancel the request.</param>
        /// <returns>HttpResponseMessage.</returns>
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
#endif

        private async Task<Stream> GetStream(string url, CancellationToken cancellationToken)
        {
            var response = await GetAsync(url, cancellationToken);
#if __EMBY__
            return response.Content;
#else
            return await response.Content.ReadAsStreamAsync(cancellationToken);
#endif
        }
    }
}