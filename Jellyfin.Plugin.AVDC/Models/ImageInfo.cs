namespace Jellyfin.Plugin.AVDC.Models
{
    public class ImageInfo
    {
        public ImageInfo()
        {
            Width = null;
            Height = null;
        }

        public int? Width { get; set; }
        public int? Height { get; set; }

        public bool Valid()
        {
            return Width > 0 && Height > 0;
        }
    }
}