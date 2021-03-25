using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ActressProvider : BaseProvider, IRemoteMetadataProvider<Person, PersonLookupInfo>, IHasOrder
    {
        public ActressProvider(IHttpClientFactory httpClientFactory, ILogger<ActressProvider> logger) : base(
            httpClientFactory, logger)
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Plugin.Instance.Name;

        public async Task<MetadataResult<Person>> GetMetadata(PersonLookupInfo info,
            CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetMetadata for actress: {Name}", info.Name);

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
            Logger.LogInformation("[AVDC] SearchResults for actress: {Name}", info.Name);

            var name = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(name)) name = info.Name;

            var actress = await ApiClient.GetActress(name, cancellationToken);
            if (!actress.Valid()) return new List<RemoteSearchResult>();

            return new List<RemoteSearchResult>
            {
                new()
                {
                    Name = actress.Name,
                    ProviderIds = new Dictionary<string, string> {{Name, actress.Name}},
                    ImageUrl = ApiClient.GetActressImageUrl(actress.Name)
                }
            };
        }
    }
}