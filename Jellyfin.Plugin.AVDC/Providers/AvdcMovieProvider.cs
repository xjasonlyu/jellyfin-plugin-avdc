using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class AvdcMovieProvider : AvdcBaseProvider, IRemoteMetadataProvider<Movie, MovieInfo>, IHasOrder
    {
        public AvdcMovieProvider(IHttpClientFactory httpClientFactory,
            IJsonSerializer jsonSerializer,
            ILogger<AvdcMovieProvider> logger) : base(httpClientFactory, jsonSerializer, logger)
        {
            // Empty
        }

        public int Order => 1;

        public string Name => "AVDC";

        public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info,
            CancellationToken cancellationToken)
        {
            Logger.LogInformation($"[AVDC] GetMetadata for video: {info.Name}");

            var m = await GetMetadata(info.Name, cancellationToken);
            if (m == null || string.IsNullOrEmpty(m.Vid)) return new MetadataResult<Movie>();

            var releaseDate = DateTime.Parse(m.Release);
            var studios = new List<string>();
            if (!string.IsNullOrWhiteSpace(m.Studio)) studios.Add(m.Studio);

            var result = new MetadataResult<Movie>
            {
                Item = new Movie
                {
                    Name = $"{m.Vid} {m.Title}",
                    Overview = m.Overview,
                    OriginalTitle = m.Title,
                    Genres = m.Genres.ToArray(),
                    PremiereDate = releaseDate,
                    ProductionYear = releaseDate.Year,
                    SortName = m.Vid,
                    ForcedSortName = m.Vid,
                    ExternalId = m.Vid,
                    Studios = studios.ToArray(),
                    OfficialRating = "XXX"
                }
            };
            AddStars(result, m.Stars);

            result.QueriedById = true;
            result.HasMetadata = true;
            return result;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
            MovieInfo info, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"[AVDC] SearchMetadata for video: {info.Name}");

            var m = await GetMetadata(info.Name, cancellationToken);
            if (m == null || string.IsNullOrEmpty(m.Vid)) return new List<RemoteSearchResult>();

            return new List<RemoteSearchResult>
            {
                new()
                {
                    Name = $"{m.Vid} {m.Title}",
                    ProductionYear = m.Year,
                    ImageUrl = $"{Config.AvdcServer}{AvdcApi.Primary}{m.Vid}"
                }
            };
        }

        private void AddStars(MetadataResult<Movie> result, List<string> stars)
        {
            foreach (var name in stars)
                result.AddPerson(new PersonInfo
                {
                    Name = name,
                    Type = "Actor",
                    ImageUrl = $"{Config.AvdcServer}{AvdcApi.People}{name}"
                });
        }
    }
}