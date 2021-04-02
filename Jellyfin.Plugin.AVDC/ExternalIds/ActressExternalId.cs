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
        public string Name => Constants.Avdc;
#else
        public string ProviderName => Constants.Avdc;
#endif
        public string Key => Constants.Avdc;

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