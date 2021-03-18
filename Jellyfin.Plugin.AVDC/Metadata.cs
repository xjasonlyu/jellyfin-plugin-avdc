using System.Collections.Generic;

namespace Jellyfin.Plugin.AVDC
{
    public class Metadata
    {
        public string Vid { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Studio { get; set; }
        public string Release { get; set; }
        public int Year { get; set; }
        public List<string> Stars { get; set; }
        public List<string> Genres { get; set; }
    }
}