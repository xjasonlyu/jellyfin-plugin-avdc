using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AVDC.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        ///     The default AVDC server address.
        /// </summary>
        public const string DefaultServer = "http://avdc.internal:5000";

        private string _server = DefaultServer;

        /// <summary>
        ///     Gets or sets the configured AVDC server address.
        /// </summary>
        public string Server
        {
            get => _server;
            set => _server = value.TrimEnd('/');
        }

        /// <summary>
        ///     Gets or sets token for AVDC server API.
        /// </summary>
        public string Token { get; set; }
    }
}