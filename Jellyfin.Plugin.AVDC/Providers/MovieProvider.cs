using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AVDC.Helpers;
using Jellyfin.Plugin.AVDC.Models;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
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
    public class MovieProvider : BaseProvider, IRemoteMetadataProvider<Movie, MovieInfo>, IHasOrder
    {
#if __EMBY__
        public MovieProvider(IHttpClient httpClient, IJsonSerializer jsonSerializer, ILogManager logManager) : base(
            httpClient,
            jsonSerializer,
            logManager.CreateLogger<MovieProvider>())
#else
        public MovieProvider(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer,
            ILogger<MovieProvider> logger) : base(
            httpClientFactory, jsonSerializer, logger)
#endif
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Constant.Avdc;

        public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info,
            CancellationToken cancellationToken)
        {
            var vid = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(vid)) vid = ExtractVid(info.Name);

#if __EMBY__
            Logger.Info("[AVDC] GetMetadata for video: {0}", vid);
#else
            Logger.LogInformation("[AVDC] GetMetadata for video: {Vid}", vid);
#endif

            var m = await ApiClient.GetMetadata(vid, cancellationToken);
            if (!m.Valid()) return new MetadataResult<Movie>();

            // Add `ChineseSubtitle` Genre
            var genres = m.Genres.ToList();
            if (!genres.Contains(Genres.ChineseSubtitle) && Genres.HasChineseSubtitle(info))
                genres.Add(Genres.ChineseSubtitle);

            // Create Studios
            var studios = new List<string>();
            if (!string.IsNullOrWhiteSpace(m.Studio)) studios.Add(m.Studio);

            // Use Series or Label as Tagline
            var tagline = !string.IsNullOrWhiteSpace(m.Series) ? m.Series :
                !string.IsNullOrWhiteSpace(m.Label) ? m.Label : string.Empty;

            var result = new MetadataResult<Movie>
            {
                Item = new Movie
                {
                    Name = FormatName(m),
                    OriginalTitle = m.Title,
                    Overview = m.Overview,
                    Tagline = tagline,
                    Genres = genres.ToArray(),
                    Studios = studios.ToArray(),
                    PremiereDate = m.Release,
                    ProductionYear = m.Release.Year,
                    OfficialRating = "XXX"
                },
                HasMetadata = true
            };
            result.Item.SetProviderId(Name, m.Vid);

            // Set All External Links
            SetProviderIds(result.Item, m.Providers, m.Sources);

            // Add Director
            if (!string.IsNullOrWhiteSpace(m.Director))
                result.AddPerson(new PersonInfo
                {
                    Name = m.Director,
                    Type = PersonType.Director
                });

            // Add Actresses
            foreach (var name in m.Actresses)
            {
                var actress = await ApiClient.GetActress(name, cancellationToken);

                var url = actress.Valid() ? ApiClient.GetActressImageUrl(actress.Name) : string.Empty;

                result.AddPerson(new PersonInfo
                {
                    Name = name,
                    Type = PersonType.Actor,
                    ImageUrl = url
                });
            }

            return result;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
            MovieInfo info, CancellationToken cancellationToken)
        {
            var vid = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(vid)) vid = ExtractVid(info.Name);

#if __EMBY__
            Logger.Info("[AVDC] SearchResults for video: {0}", vid);
#else
            Logger.LogInformation("[AVDC] SearchResults for video: {Vid}", vid);
#endif

            var m = await ApiClient.GetMetadata(vid, cancellationToken);
            if (!m.Valid()) return new List<RemoteSearchResult>();

            var result = new RemoteSearchResult
            {
                Name = FormatName(m),
                SearchProviderName = Name,
                ProductionYear = m.Release.Year,
                ImageUrl = ApiClient.GetPrimaryImageUrl(m.Vid)
            };
            result.SetProviderId(Name, m.Vid);

            return new List<RemoteSearchResult> {result};
        }

        private static string FormatName(Metadata m)
        {
            return m.Vid.Contains(".") ? m.Vid : $"{m.Vid} {m.Title}";
        }
    }
}