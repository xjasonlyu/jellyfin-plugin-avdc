using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class Fc2ExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constant.Fc2;
#else
        public string ProviderName => Constant.Fc2;
#endif
        public string Key => Constant.Fc2;

        public string UrlFormatString => "https://adult.contents.fc2.com/article/{0}";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}