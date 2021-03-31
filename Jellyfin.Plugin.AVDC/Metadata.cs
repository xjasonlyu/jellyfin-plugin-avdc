using System;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.AVDC
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
            Actresses = Array.Empty<string>();
        }

        [JsonPropertyName("vid")] public string Vid { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("overview")] public string Overview { get; set; }
        [JsonPropertyName("label")] public string Label { get; set; }
        [JsonPropertyName("series")] public string Series { get; set; }
        [JsonPropertyName("studio")] public string Studio { get; set; }
        [JsonPropertyName("director")] public string Director { get; set; }
        [JsonPropertyName("release")] public DateTime Release { get; set; }
        [JsonPropertyName("genres")] public string[] Genres { get; set; }
        [JsonPropertyName("actresses")] public string[] Actresses { get; set; }

        public bool Valid()
        {
            return !string.IsNullOrWhiteSpace(Vid);
        }
    }
}