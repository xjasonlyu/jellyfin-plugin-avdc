using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
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
        public ImageProvider(IHttpClient httpClient, IJsonSerializer jsonSerializer, ILogManager logManager) : base(
            httpClient,
            jsonSerializer,
            logManager.CreateLogger<ImageProvider>())
#else
        public ImageProvider(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer,
            ILogger<ImageProvider> logger) : base(
            httpClientFactory, jsonSerializer, logger)
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
            var vid = item.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(vid)) vid = ExtractVid(item.FileNameWithoutExtension);

#if __EMBY__
            Logger.Info("[AVDC] GetImages for video: {0}", vid);
#else
            Logger.LogInformation("[AVDC] GetImages for video: {Vid}", vid);
#endif

            var m = await ApiClient.GetMetadata(vid, cancellationToken);
            if (!m.Valid()) return new List<RemoteImageInfo>();

            var imageInfo = await ApiClient.GetBackdropImageInfo(m.Vid, cancellationToken);

            return new List<RemoteImageInfo>
            {
                new RemoteImageInfo
                {
                    ProviderName = Name,
                    Type = ImageType.Primary,
                    Width = (int) (imageInfo.Height * (2.0 / 3.0)),
                    Height = imageInfo.Height,
                    RatingType = RatingType.Likes,
                    CommunityRating = 1, // default
                    Url = ApiClient.GetPrimaryImageUrl(m.Vid)
                },
                new RemoteImageInfo
                {
                    ProviderName = Name,
                    Type = ImageType.Thumb,
                    Width = imageInfo.Width,
                    Height = (int) (imageInfo.Width / (16.0 / 9.0)),
                    RatingType = RatingType.Likes,
                    CommunityRating = 1, // default
                    Url = ApiClient.GetThumbImageUrl(m.Vid)
                },
                new RemoteImageInfo
                {
                    ProviderName = Name,
                    Type = ImageType.Backdrop,
                    Width = imageInfo.Width,
                    Height = imageInfo.Height,
                    RatingType = RatingType.Likes,
                    CommunityRating = 1, // default
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
                ImageType.Thumb,
                ImageType.Backdrop
            };
        }
    }
}