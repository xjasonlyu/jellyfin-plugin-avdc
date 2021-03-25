using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.AVDC
{
    public class Actress
    {
        public Actress()
        {
            Name = string.Empty;
            Images = Array.Empty<string>();
            Sign = string.Empty;
            Height = string.Empty;
            CupSize = string.Empty;
            BloodType = string.Empty;
            Nationality = string.Empty;
            Measurements = string.Empty;
            Birthday = new DateTime();
            AvActivity = new DateTime();
        }

        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("images")] public string[] Images { get; set; }
        [JsonPropertyName("sign")] public string Sign { get; set; }
        [JsonPropertyName("height")] public string Height { get; set; }
        [JsonPropertyName("cup_size")] public string CupSize { get; set; }
        [JsonPropertyName("blood_type")] public string BloodType { get; set; }
        [JsonPropertyName("nationality")] public string Nationality { get; set; }
        [JsonPropertyName("measurements")] public string Measurements { get; set; }
        [JsonPropertyName("birthday")] public DateTime? Birthday { get; set; }
        [JsonPropertyName("av_activity")] public DateTime? AvActivity { get; set; }

        public bool Valid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Images.Any();
        }
    }

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