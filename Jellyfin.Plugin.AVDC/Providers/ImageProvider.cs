using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
#if __EMBY__
using Jellyfin.Plugin.AVDC.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Logging;

#else
using System.Net.Http;
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
    {
#if __EMBY__
        public ImageProvider(IHttpClient httpClient, ILogManager logManager) : base(httpClient,
            logManager.CreateLogger<ImageProvider>())
#else
        public ImageProvider(IHttpClientFactory httpClientFactory, ILogger<ImageProvider> logger) : base(
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
            Logger.Info("[AVDC] GetImages for video: {0}", item.Name);
#else
        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetImages for video: {Name}", item.Name);
#endif
            var vid = item.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(vid)) vid = Utility.ExtractVid(item.FileNameWithoutExtension);

            var m = await ApiClient.GetMetadata(vid, cancellationToken);
            if (!m.Valid()) return new List<RemoteImageInfo>();

            return new List<RemoteImageInfo>
            {
                new RemoteImageInfo
                {
                    ProviderName = Name,
                    Type = ImageType.Primary,
                    Url = ApiClient.GetPrimaryImageUrl(m.Vid)
                },
                new RemoteImageInfo
                {
                    ProviderName = Name,
                    Type = ImageType.Backdrop,
                    Url = ApiClient.GetBackdropImageUrl(m.Vid)
                }
            };
        }

        public bool Supports(BaseItem item)
        {
            return item is Movie;
        }

        public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
        {
            return new List<ImageType>
            {
                ImageType.Primary,
                ImageType.Backdrop
            };
        }
    }
}