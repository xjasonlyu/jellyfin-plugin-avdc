using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AVDC.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public PluginConfiguration()
        {
            Server = "http://avdc-api:5000";
            Token = string.Empty;
            EnableRemoteImageInfo = true;
        }

        /// <summary>
        ///     Gets or sets the AVDC server URL.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the AVDC server API token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Switch the AVDC server EnableRemoteImageInfo value.
        /// </summary>
        public bool EnableRemoteImageInfo { get; set; }
    }
}