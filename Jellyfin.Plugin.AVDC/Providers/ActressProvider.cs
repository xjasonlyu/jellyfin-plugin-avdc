using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AVDC.Models;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
#if __EMBY__
using Jellyfin.Plugin.AVDC.Extensions;
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
        public ActressProvider(IHttpClient httpClient, IJsonSerializer jsonSerializer, ILogManager logManager) : base(
            httpClient,
            jsonSerializer,
            logManager.CreateLogger<ActressProvider>())
#else
        public ActressProvider(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer,
            ILogger<ActressProvider> logger) : base(
            httpClientFactory, jsonSerializer, logger)
#endif
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Constants.Avdc;

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

            var result = new MetadataResult<Person>
            {
                Item = new Person
                {
                    Name = actress.Name,
                    PremiereDate = actress.Birthday,
                    ProductionYear = actress.Birthday?.Year,
                    ProductionLocations = locations.ToArray(),
                    Overview = FormatOverview(actress)
                },
                HasMetadata = true
            };
            result.Item.SetProviderId(Name, actress.Name);

            return result;
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

            var result = new RemoteSearchResult
            {
                Name = actress.Name,
                SearchProviderName = Name,
                ImageUrl = ApiClient.GetActressImageUrl(actress.Name)
            };
            result.SetProviderId(Name, actress.Name);

            return new List<RemoteSearchResult> {result};
        }

        private static string FormatOverview(Actress a)
        {
            string G(string k, string v)
            {
                return !string.IsNullOrWhiteSpace(v) ? $"{k}: {v}\n" : string.Empty;
            }

            var overview = string.Empty;
            overview += G("身高", a.Height);
            overview += G("血型", a.Blood_Type);
            overview += G("罩杯", a.Cup_Size);
            overview += G("三围", a.Measurements);
            return overview;
        }
    }
}