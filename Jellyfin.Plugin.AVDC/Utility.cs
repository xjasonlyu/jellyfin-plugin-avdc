using System.IO;
using System.Linq;
using MediaBrowser.Controller.Providers;

namespace Jellyfin.Plugin.AVDC
{
    public static class Utility
    {
        public static bool HasChineseSubtitle(MovieInfo info)
        {
            var filename = Path.GetFileNameWithoutExtension(info.Path);

            // Simply check if filename contains `-C`
            return filename?.ToUpper().Replace("CD", "").Contains("-C") ?? false;
        }

        public static string ExtractVid(string name)
        {
            return name.Split(' ', 2).FirstOrDefault();
        } 
    }
}