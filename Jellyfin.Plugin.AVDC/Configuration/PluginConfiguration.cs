using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AVDC.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public PluginConfiguration()
        {
            AvdcServer = "http://avdc.internal:5000";
        }

        public string AvdcServer { get; set; }
    }
}