using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
    {
        public ImageProvider(IHttpClientFactory httpClientFactory, ILogger<ImageProvider> logger) : base(
            httpClientFactory, logger)
        {
            // Empty
        }

        public int Order => 1;
        public string Name => Plugin.Instance.Name;

        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetImages for video: {Name}", item.Name);

            var vid = item.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(vid)) vid = Utility.ExtractVid(item.FileNameWithoutExtension);

            var m = await ApiClient.GetMetadata(vid, cancellationToken);
            if (!m.Valid()) return new List<RemoteImageInfo>();

            return new List<RemoteImageInfo>
            {
                new()
                {
                    ProviderName = Name,
                    Type = ImageType.Primary,
                    Url = ApiClient.GetPrimaryImageUrl(m.Vid)
                },
                new()
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