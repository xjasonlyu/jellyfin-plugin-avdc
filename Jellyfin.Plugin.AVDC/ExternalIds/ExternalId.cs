using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.AVDC.ExternalIds
{
    public class ExternalId : IExternalId
    {
        public string ProviderName => Plugin.Instance.Name;

        public string Key => Plugin.Instance.Name;

        public string UrlFormatString => null;

        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie;
        }
    }
}