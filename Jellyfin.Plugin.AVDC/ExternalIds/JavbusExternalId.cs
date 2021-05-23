using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class JavbusExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constant.JavBus;
#else
        public string ProviderName => Constant.JavBus;
#endif
        public string Key => Constant.JavBus;

        public string UrlFormatString => "https://www.javbus.com/ja/{0}";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}