using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AVDC.Configuration;
using Jellyfin.Plugin.AVDC.Models;
#if __EMBY__
using System.Net;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;
using HttpRequestOptions = MediaBrowser.Common.Net.HttpRequestOptions;

#else
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.AVDC;

public sealed class ApiClient
{
    private const string Actress = "/actress/";
    private const string Metadata = "/metadata/";
    private const string ActressImage = "/image/actress/";
    private const string PrimaryImage = "/image/primary/";
    private const string ThumbImage = "/image/thumb/";
    private const string BackdropImage = "/image/backdrop/";
    private const string RemoteImage = "/image/remote/";
    private const string ActressImageInfo = "/imageinfo/actress/";
    private const string BackdropImageInfo = "/imageinfo/backdrop/";
    private const string RemoteImageInfo = "/imageinfo/remote/";

#if __EMBY__
    private readonly IHttpClient _httpClient;
#else
        private readonly IHttpClientFactory _httpClientFactory;
#endif
    private readonly ILogger _logger;

#if __EMBY__
    public ApiClient(IHttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
#else
        public ApiClient(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
#endif
        _logger = logger;
    }

    private static PluginConfiguration Config => Plugin.Instance?.Configuration ?? new PluginConfiguration();

    private static string ComposeUrl(string pathAndQuery)
    {
        return $"{Config.Server.TrimEnd('/')}{pathAndQuery}";
    }

    private static string GetActressUrl(string name)
    {
        return ComposeUrl($"{Actress}{name}");
    }

    private static string GetMetadataUrl(string vid)
    {
        return ComposeUrl($"{Metadata}{vid}");
    }

    private static string GetActressImageInfoUrl(string name, int index)
    {
        return ComposeUrl($"{ActressImageInfo}{name}/{index}");
    }

    private static string GetBackdropImageInfoUrl(string vid)
    {
        return ComposeUrl($"{BackdropImageInfo}{vid}");
    }

    public static string GetActressImageUrl(string name, int index = 0)
    {
        return ComposeUrl($"{ActressImage}{name}/{index}");
    }

    public static string GetPrimaryImageUrl(string vid)
    {
        return ComposeUrl($"{PrimaryImage}{vid}");
    }

    public static string GetThumbImageUrl(string vid)
    {
        return ComposeUrl($"{ThumbImage}{vid}");
    }

    public static string GetBackdropImageUrl(string vid)
    {
        return ComposeUrl($"{BackdropImage}{vid}");
    }

    public static string GetRemoteImageUrl(string name, string url, double scale = 0)
    {
        return ComposeUrl($"{RemoteImage}{name}?scale={scale}&url={Uri.EscapeDataString(url)}");
    }

    private static string GetRemoteImageInfoUrl(string name, string url)
    {
        return ComposeUrl($"{RemoteImageInfo}{name}?url={Uri.EscapeDataString(url)}");
    }

    public async Task<ImageInfo> GetRemoteImageInfo(string name, string url, CancellationToken cancellationToken)
    {
        if (!Config.EnableRemoteImageInfo) return new ImageInfo();

        try
        {
            var contentStream = await GetStream(GetRemoteImageInfoUrl(name, url), cancellationToken);
            return JsonSerializer.Deserialize<ImageInfo>(contentStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception e)
        {
#if __EMBY__
            _logger.Error("[AVDC] GetRemoteImageInfo for {0} failed: {1}", name, e.Message);
#else
                _logger.LogError("[AVDC] GetRemoteImageInfo for {Name} failed: {Message}", name, e.Message);
#endif
            return new ImageInfo();
        }
    }

    /// <summary>
    ///     Get Actress Image Info by Name.
    /// </summary>
    /// <param name="name">Actress Name.</param>
    /// <param name="cancellationToken">Used to cancel the request.</param>
    /// <param name="index">Image Index Number.</param>
    /// <returns>ImageInfo.</returns>
    public async Task<ImageInfo> GetActressImageInfo(string name, CancellationToken cancellationToken,
        int index = 0)
    {
        try
        {
            var contentStream = await GetStream(GetActressImageInfoUrl(name, index), cancellationToken);
            return JsonSerializer.Deserialize<ImageInfo>(contentStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception e)
        {
#if __EMBY__
            _logger.Error("[AVDC] GetActressImageInfo for {0} failed: {1}", name, e.Message);
#else
                _logger.LogError("[AVDC] GetActressImageInfo for {Name} failed: {Message}", name, e.Message);
#endif
            return new ImageInfo();
        }
    }

    /// <summary>
    ///     Get Backdrop Image Info by Vid.
    /// </summary>
    /// <param name="vid">Video ID.</param>
    /// <param name="cancellationToken">Used to cancel the request.</param>
    /// <returns>ImageInfo.</returns>
    public async Task<ImageInfo> GetBackdropImageInfo(string vid, CancellationToken cancellationToken)
    {
        try
        {
            var contentStream = await GetStream(GetBackdropImageInfoUrl(vid), cancellationToken);
            return JsonSerializer.Deserialize<ImageInfo>(contentStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception e)
        {
#if __EMBY__
            _logger.Error("[AVDC] GetActressImageInfo for {0} failed: {1}", vid, e.Message);
#else
                _logger.LogError("[AVDC] GetActressImageInfo for {Vid} failed: {Message}", vid, e.Message);
#endif
            return new ImageInfo();
        }
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
            return JsonSerializer.Deserialize<Metadata>(contentStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
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
            return JsonSerializer.Deserialize<Actress>(contentStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
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
            CancellationToken = cancellationToken,
            EnableDefaultUserAgent = true,
            TimeoutMs = 60000
        };

        // Add Auth Token Header
        if (!string.IsNullOrEmpty(Config.Token) && url.StartsWith(Config.Server))
            options.RequestHeaders.Add("Authorization", $"Bearer {Config.Token}");

        var response = await _httpClient.GetResponse(options).ConfigureAwait(false);
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
            httpClient.Timeout = TimeSpan.FromSeconds(60);

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