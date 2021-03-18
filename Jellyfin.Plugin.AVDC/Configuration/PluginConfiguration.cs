using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AVDC.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public string AvdcServer { get; set; }

        public PluginConfiguration()
        {
            AvdcServer = "http://avdc.internal:5000";
        }
    }
}
