using System.IO;
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
            return $"{m.Vid} {m.Title}";
        }

        public static bool HasChineseSubtitle(MovieInfo info)
        {
            var filename = Path.GetFileNameWithoutExtension(info.Path);

            // Simply check if filename contains `-C`
            return filename?.ToUpper().Replace("CD", "").Contains("-C") ?? false;
        }
    }
}