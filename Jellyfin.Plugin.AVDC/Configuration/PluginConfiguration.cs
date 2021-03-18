using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AVDC.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public string AVDCServer { get; set; }

        public PluginConfiguration()
        {
            AVDCServer = "http://avdc.internal:5000";
        }
    }
}
