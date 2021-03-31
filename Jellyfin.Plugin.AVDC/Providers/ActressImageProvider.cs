using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
#if __EMBY__
using Jellyfin.Plugin.AVDC.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Configuration;

#else
using System.Net.Http;
using Microsoft.Extensions.Logging;
#endif
namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ActressImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
    {
#if __EMBY__
        public ActressImageProvider(IHttpClient httpClient, ILogManager logManager) : base(httpClient,
            logManager.CreateLogger<ActressImageProvider>())
#else
        public ActressImageProvider(IHttpClientFactory httpClientFactory, ILogger<ActressImageProvider> logger) : base(
            httpClientFactory, logger)
#endif
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Plugin.Instance.Name;

#if __EMBY__
        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, LibraryOptions libraryOptions,
            CancellationToken cancellationToken)
        {
            Logger.Info("[AVDC] GetImages for actress: {0}", item.Name);
#else
        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetImages for actress: {Name}", item.Name);
#endif
            var name = item.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(name)) name = item.Name;

            var actress = await ApiClient.GetActress(name, cancellationToken);
            if (!actress.Valid()) return new List<RemoteImageInfo>();

            return new List<RemoteImageInfo>
            {
                new RemoteImageInfo
                {
                    ProviderName = Name,
                    Type = ImageType.Primary,
                    Url = ApiClient.GetActressImageUrl(actress.Name)
                }
            };
        }

        public bool Supports(BaseItem item)
        {
            return item is Person;
        }

        public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
        {
            return new List<ImageType>
            {
                ImageType.Primary
            };
        }
    }
}