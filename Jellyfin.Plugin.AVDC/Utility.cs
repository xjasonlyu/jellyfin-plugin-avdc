using System;
using System.IO;
using System.Text.RegularExpressions;
using Jellyfin.Plugin.AVDC.Models;
using MediaBrowser.Controller.Providers;

namespace Jellyfin.Plugin.AVDC
{
    public static class Utility
    {
        public static string ExtractVid(string name)
        {
            return name; // do nothing
        }

        public static string FormatName(Metadata m)
        {
            return m.Vid.Contains(".") ? m.Vid : $"{m.Vid} {m.Title}";
        }

        public static string FormatOverview(Actress a)
        {
            string G(string k, string v)
            {
                return !string.IsNullOrWhiteSpace(v) ? $"{k}: {v}\n" : string.Empty;
            }

            var overview = string.Empty;
            overview += G("身高", a.Height);
            // overview += G("星座", a.Sign);
            overview += G("血型", a.BloodType);
            overview += G("罩杯", a.CupSize);
            overview += G("三围", a.Measurements);
            return overview;
        }

        public static bool HasChineseSubtitle(MovieInfo info)
        {
#if __EMBY__
            var filename = Path.GetFileNameWithoutExtension(info.Name);
#else
            var filename = Path.GetFileNameWithoutExtension(info.Path);
#endif
            var r = new Regex(@"-cd\d+$",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            filename = r.Replace(filename ?? string.Empty, string.Empty);

            return filename.ToUpper().Substring(Math.Max(0, filename.Length - 2)).Equals("-C");
        }
    }
}