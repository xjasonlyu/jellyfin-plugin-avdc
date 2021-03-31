using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class ExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Plugin.Instance.Name;
#else
        public string ProviderName => Plugin.Instance.Name;
#endif
        public string Key => Plugin.Instance.Name;

        public string UrlFormatString => null;

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}