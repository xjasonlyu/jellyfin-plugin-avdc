using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class MgstageExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constant.Mgstage;
#else
        public string ProviderName => Constant.Mgstage;
#endif
        public string Key => Constant.Mgstage;

        public string UrlFormatString => "https://www.mgstage.com/product/product_detail/{0}/";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}