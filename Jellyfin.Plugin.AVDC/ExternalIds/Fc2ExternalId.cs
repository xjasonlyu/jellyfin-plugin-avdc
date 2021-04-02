using Jellyfin.Plugin.AVDC.Providers;
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
        public string Name => ProviderNames.Fc2;
#else
        public string ProviderName => ProviderNames.Fc2;
#endif
        public string Key => ProviderNames.Fc2;

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