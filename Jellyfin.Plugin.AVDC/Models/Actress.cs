using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.AVDC.Models
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
}