using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AVDC.Providers
{
    public class MovieProvider : BaseProvider, IRemoteMetadataProvider<Movie, MovieInfo>, IHasOrder
    {
        public MovieProvider(IHttpClientFactory httpClientFactory, ILogger<MovieProvider> logger) : base(
            httpClientFactory, logger)
        {
            // Empty
        }

        public int Order => 1;

        public string Name => Plugin.Instance.Name;

        public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info,
            CancellationToken cancellationToken)
        {
            Logger.LogInformation("[AVDC] GetMetadata for video: {Name}", info.Name);

            var vid = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(vid)) vid = Utility.ExtractVid(info.Name);

            var m = await GetMetadata(vid, cancellationToken);
            if (!m.Valid()) return new MetadataResult<Movie>();

            // Add `中文字幕` Genre
            var genres = m.Genres.ToList();
            if (Utility.HasChineseSubtitle(info) && !genres.Contains("中文字幕"))
                genres.Add("中文字幕");

            // Create Studios
            var studios = new List<string>();
            if (!string.IsNullOrWhiteSpace(m.Studio)) studios.Add(m.Studio);

            // Use Series or Label as Tagline
            var tagline = !string.IsNullOrWhiteSpace(m.Series) ? m.Series : m.Label;

            var result = new MetadataResult<Movie>
            {
                Item = new Movie
                {
                    Name = Utility.FormatName(m),
                    OriginalTitle = m.Title,
                    Overview = m.Overview,
                    Tagline = tagline,
                    Genres = genres.ToArray(),
                    Studios = studios.ToArray(),
                    PremiereDate = m.Release,
                    ProductionYear = m.Release.Year,
                    ProviderIds = new Dictionary<string, string> {{Name, m.Vid}},
                    OfficialRating = "XXX"
                },
                HasMetadata = true
            };

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
                var actress = await GetActress(name, cancellationToken);

                var url = !actress.Valid()
                    ? string.Empty
                    : $"{Config.Server}{ApiPath.ActressImage}{actress.Name}";

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
            Logger.LogInformation("[AVDC] SearchResults for video: {Name}", info.Name);

            var vid = info.GetProviderId(Name);
            if (string.IsNullOrWhiteSpace(vid)) vid = Utility.ExtractVid(info.Name);

            var m = await GetMetadata(vid, cancellationToken);
            if (!m.Valid()) return new List<RemoteSearchResult>();

            return new List<RemoteSearchResult>
            {
                new()
                {
                    Name = Utility.FormatName(m),
                    ProductionYear = m.Release.Year,
                    ProviderIds = new Dictionary<string, string> {{Name, m.Vid}},
                    ImageUrl = $"{Config.Server}{ApiPath.PrimaryImage}{m.Vid}"
                }
            };
        }
    }
}