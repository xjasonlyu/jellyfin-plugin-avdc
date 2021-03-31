using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

#else
using System.Net.Http;
using Microsoft.Extensions.Logging;
#endif
namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ActressProvider : BaseProvider, IRemoteMetadataProvider<Person, PersonLookupInfo>, IHasOrder
    {
#if __EMBY__
        public ActressProvider(IHttpClient httpClient, ILogger logger) : base(httpClient, logger)
#else
        public ActressProvider(IHttpClientFactory httpClientFactory, ILogger<ActressProvider> logger) : base(
            httpClientFactory, logger)
#endif
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Plugin.Instance.Name;

        public async Task<MetadataResult<Person>> GetMetadata(PersonLookupInfo info,
            CancellationToken cancellationToken)
        {
#if __EMBY__
            Logger.Info("[AVDC] GetMetadata for actress: {0}", info.Name);
#else
            Logger.LogInformation("[AVDC] GetMetadata for actress: {Name}", info.Name);
#endif
            var name = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(name)) name = info.Name;

            var actress = await ApiClient.GetActress(name, cancellationToken);
            if (!actress.Valid()) return new MetadataResult<Person>();

            var locations = new List<string>();
            if (!string.IsNullOrWhiteSpace(actress.Nationality))
                locations.Add(actress.Nationality);

            return new MetadataResult<Person>
            {
                Item = new Person
                {
                    Name = actress.Name,
                    PremiereDate = actress.Birthday,
                    ProductionYear = actress.Birthday?.Year,
                    ProductionLocations = locations.ToArray(),
                    Overview = Utility.FormatOverview(actress),
                    ProviderIds = new Dictionary<string, string> {{Name, actress.Name}}
                },
                HasMetadata = true
            };
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
            PersonLookupInfo info, CancellationToken cancellationToken)
        {
#if __EMBY__
            Logger.Info("[AVDC] SearchResults for actress: {0}", info.Name);
#else
            Logger.LogInformation("[AVDC] SearchResults for actress: {Name}", info.Name);
#endif
            var name = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(name)) name = info.Name;

            var actress = await ApiClient.GetActress(name, cancellationToken);
            if (!actress.Valid()) return new List<RemoteSearchResult>();

            return new List<RemoteSearchResult>
            {
                new RemoteSearchResult
                {
                    Name = actress.Name,
                    ProviderIds = new Dictionary<string, string> {{Name, actress.Name}},
                    ImageUrl = ApiClient.GetActressImageUrl(actress.Name)
                }
            };
        }
    }
}