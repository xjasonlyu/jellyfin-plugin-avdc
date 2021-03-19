using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class ActressProvider : BaseProvider, IRemoteMetadataProvider<Person, PersonLookupInfo>, IHasOrder
    {
        public ActressProvider(IHttpClientFactory httpClientFactory,
            IJsonSerializer jsonSerializer,
            ILogger<ActressProvider> logger) : base(httpClientFactory, jsonSerializer, logger)
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Plugin.Instance.Name;

        public async Task<MetadataResult<Person>> GetMetadata(PersonLookupInfo info,
            CancellationToken cancellationToken)
        {
            Logger.LogInformation($"[AVDC] GetMetadata for actress: {info.Name}");

            var actressId = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(actressId)) actressId = info.Name;

            var actress = await GetActress(actressId, cancellationToken);
            if (actress == null || string.IsNullOrWhiteSpace(actress.Name) || !actress.Images.Any())
                return new MetadataResult<Person>();

            return new MetadataResult<Person>
            {
                Item = new Person
                {
                    Name = actress.Name,
                    ProviderIds = new Dictionary<string, string> {{Name, actress.Name}}
                },
                HasMetadata = true
            };
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
            PersonLookupInfo info, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"[AVDC] SearchResults for actress: {info.Name}");

            var actressId = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(actressId)) actressId = info.Name;

            var actress = await GetActress(actressId, cancellationToken);
            if (actress == null || string.IsNullOrWhiteSpace(actress.Name) || !actress.Images.Any())
                return new List<RemoteSearchResult>();

            return new List<RemoteSearchResult>
            {
                new()
                {
                    Name = actress.Name,
                    ProviderIds = new Dictionary<string, string> {{Name, actress.Name}},
                    ImageUrl = $"{Config.AvdcServer}{ApiPath.ActressImage}{actress.Name}"
                }
            };
        }
    }
}