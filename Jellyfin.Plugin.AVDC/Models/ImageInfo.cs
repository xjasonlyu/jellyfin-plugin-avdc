namespace Jellyfin.Plugin.AVDC.Models
{
    public class ImageInfo
    {
        public ImageInfo()
        {
            Width = 0;
            Height = 0;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool Valid()
        {
            return true; // always valid
        }
    }
}