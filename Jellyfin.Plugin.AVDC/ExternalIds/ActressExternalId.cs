using Jellyfin.Plugin.AVDC.Providers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class ActressExternalId : IExternalId
    {
#if __EMBY__
        public string Name => ProviderNames.Avdc;
#else
        public string ProviderName => ExternalIdNames.Avdc;
#endif
        public string Key => ProviderNames.Avdc;

        public string UrlFormatString => null;

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Person;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Person;
        }
    }
}