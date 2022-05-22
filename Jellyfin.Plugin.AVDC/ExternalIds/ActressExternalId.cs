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
        public string Name => Constant.Avdc;
#else
        public string ProviderName => Constant.Avdc;
#endif
        public string Key => Constant.Avdc;

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