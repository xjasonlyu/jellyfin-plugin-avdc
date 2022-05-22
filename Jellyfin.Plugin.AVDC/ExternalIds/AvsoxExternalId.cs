using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class AvsoxExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constant.Avsox;
#else
        public string ProviderName => Constant.Avsox;
#endif
        public string Key => Constant.Avsox;

        public string UrlFormatString => "https://avsox.website/ja/movie/{0}";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}