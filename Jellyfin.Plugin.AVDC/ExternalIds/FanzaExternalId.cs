using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class FanzaExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constant.Fanza;
#else
        public string ProviderName => Constant.Fanza;
#endif
        public string Key => Constant.Fanza;

        public string UrlFormatString => "https://www.dmm.co.jp/digital/videoa/-/detail/=/cid={0}/";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}