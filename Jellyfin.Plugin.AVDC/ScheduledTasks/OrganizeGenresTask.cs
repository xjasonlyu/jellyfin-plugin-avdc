using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AVDC.Extensions;
using Jellyfin.Plugin.AVDC.Helpers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Tasks;
#if __EMBY__
using MediaBrowser.Model.Logging;

#else
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.AVDC.ScheduledTasks
{
    public class OrganizeGenresTask : IScheduledTask
    {
        private readonly ILibraryManager _libraryManager;
        private readonly ILogger _logger;

#if __EMBY__
        public OrganizeGenresTask(ILogManager logManager, ILibraryManager libraryManager)
        {
            _logger = logManager.CreateLogger<OrganizeGenresTask>();
            _libraryManager = libraryManager;
        }
#else
        public OrganizeGenresTask(ILogger<OrganizeGenresTask> logger, ILibraryManager libraryManager)
        {
            _logger = logger;
            _libraryManager = libraryManager;
        }
#endif

        public string Key => $"{Constant.Avdc}OrganizeGenres";

        public string Name => "Organize Genres";

        public string Description => "Organize metadata genres provided by AVDC in library.";

        public string Category => Constant.Avdc;

        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            await Task.Yield();
            progress?.Report(0);

            var items = _libraryManager.GetItemList(new InternalItemsQuery
            {
                MediaTypes = new[] {MediaType.Video},
                IncludeItemTypes = new[] {nameof(Movie)}
            }).Where(item => item.ProviderIds.ContainsKey(Constant.Avdc)).ToList();

            foreach (var (idx, item) in items.WithIndex())
            {
                progress?.Report((double) idx / items.Count * 100);

                // Skip this item if empty
                if (item.Genres == null || !item.Genres.Any()) continue;

                var genres = item.Genres.ToList();

                // Replace Genres
                foreach (var genre in genres.Where(genre => Genres.Substitution.ContainsKey(genre)).ToArray())
                {
                    var value = Genres.Substitution[genre];
                    if (string.IsNullOrEmpty(value))
                        genres.Remove(genre); // should just be removed
                    else
                        genres[genres.IndexOf(genre)] = value; // replace
                }

                // Add `ChineseSubtitle` Genre
                if (!genres.Contains(Genres.ChineseSubtitle) &&
                    Genres.HasChineseSubtitle(item.FileNameWithoutExtension))
                    genres.Add(Genres.ChineseSubtitle);

                // Remove Duplicates
                genres = genres.Distinct().ToList();

                // Skip updating item if equal
                if (item.Genres.SequenceEqual(genres, StringComparer.Ordinal)) continue;

                item.Genres = genres.ToArray();

#if __EMBY__
                _logger.Info("[AVDC] OrganizeGenres for video: {0}", item.Name);
                _libraryManager.UpdateItem(item, item, ItemUpdateType.MetadataEdit);
#else
                _logger.LogInformation("[AVDC] OrganizeGenres for video: {Name}", item.Name);
                await _libraryManager
                    .UpdateItemAsync(item, item, ItemUpdateType.MetadataEdit, cancellationToken)
                    .ConfigureAwait(false);
#endif
            }

            progress?.Report(100);
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            yield return new TaskTriggerInfo
            {
                Type = TaskTriggerInfo.TriggerWeekly,
                DayOfWeek = DayOfWeek.Sunday,
                TimeOfDayTicks = TimeSpan.FromHours(2).Ticks
            };
        }
    }
}