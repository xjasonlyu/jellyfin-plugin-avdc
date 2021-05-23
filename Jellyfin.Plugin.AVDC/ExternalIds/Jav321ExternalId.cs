using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class Jav321ExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constant.Jav321;
#else
        public string ProviderName => Constant.Jav321;
#endif
        public string Key => Constant.Jav321;

        public string UrlFormatString => "https://www.jav321.com/video/{0}";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}