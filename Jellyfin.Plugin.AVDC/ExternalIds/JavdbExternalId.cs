using Jellyfin.Plugin.AVDC.Providers;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class JavdbExternalId : IExternalId
    {
#if __EMBY__
        public string Name => ProviderNames.JavDb;
#else
        public string ProviderName => ExternalIdNames.JavDb;
#endif
        public string Key => ProviderNames.JavDb;

        public string UrlFormatString => "{0}";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}