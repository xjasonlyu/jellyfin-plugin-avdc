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
    public class AvdcImageProvider : AvdcBaseProvider, IRemoteImageProvider, IHasOrder
    {
        public AvdcImageProvider(IHttpClientFactory httpClientFactory,
            IJsonSerializer jsonSerializer,
            ILogger<AvdcImageProvider> logger) : base(httpClientFactory, jsonSerializer, logger)
        {
            // Empty
        }

        public int Order => 1;
        public string Name => "AVDC";

        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"[AVDC] GetImages for video: {item.Name}");

            var m = await GetMetadata(item.FileNameWithoutExtension, cancellationToken);
            if (m == null || string.IsNullOrEmpty(m.Vid)) return new List<RemoteImageInfo>();

            var vid = m.Vid;

            return new List<RemoteImageInfo>
            {
                new()
                {
                    ProviderName = Name,
                    Url = $"{Config.AvdcServer}{AvdcApi.Primary}{vid}",
                    Type = ImageType.Primary
                },
                new()
                {
                    ProviderName = Name,
                    Url = $"{Config.AvdcServer}{AvdcApi.Backdrop}{vid}",
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