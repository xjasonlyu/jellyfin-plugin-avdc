using System;
using System.Linq;

namespace Jellyfin.Plugin.AVDC.Models
{
    public class Actress : BaseModel
    {
        public Actress()
        {
            Name = string.Empty;
            Images = Array.Empty<string>();
            Sign = string.Empty;
            Height = string.Empty;
            Cup_Size = string.Empty;
            Blood_Type = string.Empty;
            Nationality = string.Empty;
            Measurements = string.Empty;
            Birthday = new DateTime();
            Av_Activity = new DateTime();
        }

        public string Name { get; set; }
        public string[] Images { get; set; }
        public string Sign { get; set; }
        public string Height { get; set; }
        public string Cup_Size { get; set; }
        public string Blood_Type { get; set; }
        public string Nationality { get; set; }
        public string Measurements { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? Av_Activity { get; set; }

        public bool Valid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Images.Any();
        }
    }
}