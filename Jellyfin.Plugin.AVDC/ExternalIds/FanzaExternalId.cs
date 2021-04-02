using Jellyfin.Plugin.AVDC.Providers;
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
        public string Name => ProviderNames.Fanza;
#else
        public string ProviderName => ProviderNames.Fanza;
#endif
        public string Key => ProviderNames.Fanza;

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