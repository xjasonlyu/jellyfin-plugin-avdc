using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Dto;
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

namespace Jellyfin.Plugin.AVDC.Providers;

public class ActressImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
{
#if __EMBY__
    public ActressImageProvider(IHttpClient httpClient, ILogManager logManager) :
        base(httpClient, logManager.CreateLogger<ActressImageProvider>())
#else
        public ActressImageProvider(IHttpClientFactory httpClientFactory, ILogger<ActressImageProvider> logger) : base(
            httpClientFactory, logger)
#endif
    {
        // Empty
    }

    public int Order => 1;

    public string Name => Constant.Avdc;

#if __EMBY__
    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, LibraryOptions libraryOptions,
        CancellationToken cancellationToken)
#else
        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
#endif
    {
        var name = item.GetProviderId(Name);
        if (string.IsNullOrWhiteSpace(name)) name = item.Name;

#if __EMBY__
        Logger.Info("[AVDC] GetImages for actress: {0}", name);
#else
            Logger.LogInformation("[AVDC] GetImages for actress: {Name}", name);
#endif

        var actress = await ApiClient.GetActress(name, cancellationToken);
        if (!actress.Valid()) return new List<RemoteImageInfo>();

        var images = new List<RemoteImageInfo>();
        for (var i = 0; i < actress.Images.Length; i++)
        {
            double CalcRating(int index, int total)
            {
                return 5.0 * (total - index) / total + 5.0;
            }

            var imageInfo = await ApiClient.GetActressImageInfo(actress.Name, cancellationToken, i);
            images.Add(new RemoteImageInfo
            {
                ProviderName = Name,
                Type = ImageType.Primary,
                Width = imageInfo.Width,
                Height = imageInfo.Height,
                RatingType = RatingType.Score,
                CommunityRating = CalcRating(i, actress.Images.Length),
                VoteCount = actress.Images.Length,
                Url = ApiClient.GetActressImageUrl(actress.Name, i)
            });
        }

        return images;
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