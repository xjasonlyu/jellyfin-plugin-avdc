using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AVDC.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        private string _server = "http://avdc.internal:5000";

        public string Server
        {
            get => _server;
            set => _server = value.TrimEnd('/');
        }
    }
}