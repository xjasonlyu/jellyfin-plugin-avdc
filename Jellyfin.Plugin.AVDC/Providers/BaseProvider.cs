using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Serialization;
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
        protected BaseProvider(IHttpClient httpClientFactory,
#else
        protected BaseProvider(IHttpClientFactory httpClientFactory,
#endif
            IJsonSerializer jsonSerializer, ILogger logger)
        {
            Logger = logger;
            ApiClient = new ApiClient(httpClientFactory, jsonSerializer, logger);
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

        protected static string ExtractVid(string name)
        {
            var regex = new Regex(@"([A-Z0-9]+[_\-\.][A-Z0-9]+)", RegexOptions.IgnoreCase);
            var match = regex.Match(name);
            return !string.IsNullOrEmpty(match.Value) ? match.Value : name;
        }

        protected static string ExtractQuery(string name)
        {
            var i = name.IndexOf('?');
            return i < 0 ? string.Empty : name.Substring(i, name.Length - i);
        }

        private static string[] ProviderNames => typeof(Constant).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(x => x.GetValue(null).ToString()).ToArray();

        protected static void SetProviderIds(IHasProviderIds item, string[] providers, string[] sources)
        {
            for (var i = 0; i < Math.Min(providers.Length, sources.Length); i++)
            {
                var name = Array.Find(ProviderNames, e => e.Equals(providers[i], StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(name) && !name.Equals(Constant.Avdc))
                    item.SetProviderId(name, sources[i]);
            }
        }
    }
}