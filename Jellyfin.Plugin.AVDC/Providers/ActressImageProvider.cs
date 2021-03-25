using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ActressImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
    {
        public ActressImageProvider(IHttpClientFactory httpClientFactory, ILogger<ActressImageProvider> logger) : base(
            httpClientFactory, logger)
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Plugin.Instance.Name;

        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetImages for actress: {Name}", item.Name);

            var name = item.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(name)) name = item.Name;

            var actress = await GetActress(name, cancellationToken);
            if (!actress.Valid()) return new List<RemoteImageInfo>();

            return new List<RemoteImageInfo>
            {
                new()
                {
                    ProviderName = Name,
                    Type = ImageType.Primary,
                    Url = $"{Config.Server}{ApiPath.ActressImage}{actress.Name}"
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