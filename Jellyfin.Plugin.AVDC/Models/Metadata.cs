using System;

namespace Jellyfin.Plugin.AVDC.Models
{
    public class Metadata
    {
        public Metadata()
        {
            Vid = string.Empty;
            Title = string.Empty;
            Overview = string.Empty;
            Label = string.Empty;
            Series = string.Empty;
            Studio = string.Empty;
            Director = string.Empty;
            Release = new DateTime();
            Genres = Array.Empty<string>();
            Website = Array.Empty<string>();
            Actresses = Array.Empty<string>();
        }

        public string Vid { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Label { get; set; }
        public string Series { get; set; }
        public string Studio { get; set; }
        public string Director { get; set; }
        public DateTime Release { get; set; }
        public string[] Genres { get; set; }
        public string[] Website { get; set; }
        public string[] Actresses { get; set; }

        public bool Valid()
        {
            return !string.IsNullOrWhiteSpace(Vid);
        }
    }
}