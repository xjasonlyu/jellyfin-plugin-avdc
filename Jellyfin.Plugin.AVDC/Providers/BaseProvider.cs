using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public abstract class BaseProvider
    {
        protected readonly ApiClient ApiClient;
        protected readonly ILogger Logger;

        protected BaseProvider(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            Logger = logger;
            ApiClient = new ApiClient(httpClientFactory, logger);
        }

        public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetImageResponse for url: {Url}", url);

            return ApiClient.GetAsync(url, cancellationToken);
        }
    }
}