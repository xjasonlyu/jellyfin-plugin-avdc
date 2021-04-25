using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MediaBrowser.Controller.Providers;

namespace Jellyfin.Plugin.AVDC.Helpers
{
    public static class Genres
    {
        public const string ChineseSubtitle = "中文字幕";

        public static readonly Dictionary<string, string> Substitution =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"HD", null},
                {"4K", null},
                {"5K", null},
                {"720p", null},
                {"1080p", null},
                {"60fps", null}
            };

        public static bool HasChineseSubtitle(MovieInfo info)
        {
#if __EMBY__
            var filename = Path.GetFileNameWithoutExtension(info.Name);
#else
            var filename = Path.GetFileNameWithoutExtension(info.Path);
#endif
            return HasChineseSubtitle(filename);
        }

        public static bool HasChineseSubtitle(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return false;

            if (filename.Contains(ChineseSubtitle))
                return true;

            var r = new Regex(@"-cd\d+$",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            filename = r.Replace(filename, string.Empty);

            return filename.Substring(Math.Max(0, filename.Length - 2))
                .Equals("-C", StringComparison.OrdinalIgnoreCase);
        }
    }
}