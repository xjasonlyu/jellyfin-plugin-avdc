using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

#if !__EMBY__
using MediaBrowser.Model.Providers;

#endif

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class XslistExternalId : IExternalId
    {
#if __EMBY__
        public string Name => Constants.Xslist;
#else
        public string ProviderName => Constants.XsList;
#endif
        public string Key => Constants.Xslist;

        public string UrlFormatString => "{0}";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Person;
#endif

        public bool Supports(IHasProviderIds item)
        {
            return item is Person;
        }
    }
}