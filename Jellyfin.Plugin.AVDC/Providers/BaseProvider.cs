using System.Threading;
using System.Threading.Tasks;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

#else
using System.Net.Http;
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.AVDC.Providers
{
    public abstract class BaseProvider
    {
        protected readonly ApiClient ApiClient;
        protected readonly ILogger Logger;

#if __EMBY__
        protected BaseProvider(IHttpClient httpClientFactory, ILogger logger)
#else
        protected BaseProvider(IHttpClientFactory httpClientFactory, ILogger logger)
#endif
        {
            Logger = logger;
            ApiClient = new ApiClient(httpClientFactory, logger);
        }

#if __EMBY__
        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            Logger.Info("[AVDC] GetImageResponse for url: {0}", url);

            return ApiClient.GetAsync(url, cancellationToken);
        }
#else
        public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetImageResponse for url: {Url}", url);

            return ApiClient.GetAsync(url, cancellationToken);
        }
#endif
    }
}