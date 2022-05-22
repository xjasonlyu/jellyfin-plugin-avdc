using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class ArzonExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constant.Arzon;
#else
        public string ProviderName => Constant.Arzon;
#endif
        public string Key => Constant.Arzon;

        public string UrlFormatString => "https://www.arzon.jp/item_{0}.html";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}