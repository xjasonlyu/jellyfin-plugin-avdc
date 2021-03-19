using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
    {
        public ImageProvider(IHttpClientFactory httpClientFactory,
            IJsonSerializer jsonSerializer,
            ILogger<ImageProvider> logger) : base(httpClientFactory, jsonSerializer, logger)
        {
            // Empty
        }

        public int Order => 1;
        public string Name => Plugin.Instance.Name;

        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"[AVDC] GetImages for video: {item.Name}");

            var name = item.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(name)) name = item.FileNameWithoutExtension;

            var m = await GetMetadata(name, cancellationToken);
            if (m == null || string.IsNullOrWhiteSpace(m.Vid)) return new List<RemoteImageInfo>();

            return new List<RemoteImageInfo>
            {
                new()
                {
                    ProviderName = Name,
                    Url = $"{Config.AvdcServer}{ApiPath.PrimaryImage}{m.Vid}",
                    Type = ImageType.Primary
                },
                new()
                {
                    ProviderName = Name,
                    Url = $"{Config.AvdcServer}{ApiPath.BackdropImage}{m.Vid}",
                    Type = ImageType.Backdrop
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