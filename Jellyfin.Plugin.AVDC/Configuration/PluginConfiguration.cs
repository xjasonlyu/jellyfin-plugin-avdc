using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AVDC.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public PluginConfiguration()
        {
            Server = "http://avdc-api:5000";
            Token = string.Empty;
        }

        /// <summary>
        ///     Gets or sets the AVDC server URL.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the AVDC server API token.
        /// </summary>
        public string Token { get; set; }
    }
}