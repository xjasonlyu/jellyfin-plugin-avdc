using System;
using System.Collections.Generic;
using Jellyfin.Plugin.AVDC.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
#if __EMBY__
using System.IO;
using MediaBrowser.Model.Drawing;

#endif

namespace Jellyfin.Plugin.AVDC
{
#if __EMBY__
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages, IHasThumbImage
#else
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
#endif
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths,
            xmlSerializer)
        {
            Instance = this;
        }

        public override string Name => Constants.Avdc;

#if __EMBY__
        public override string Description => "AVDC Metadata Provider for Emby";
#else
        public override string Description => "AVDC Metadata Provider for Jellyfin";
#endif

        public override Guid Id => Guid.Parse("001c811c-145d-4b43-94bf-f78e8d7b6afd");

        public static Plugin Instance { get; private set; }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = Name,
                    EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.configPage.html"
                }
            };
        }

#if __EMBY__
        public Stream GetThumbImage()
        {
            var type = GetType();
            return type.Assembly.GetManifestResourceStream($"{GetType().Namespace}.thumb.png");
        }

        public ImageFormat ThumbImageFormat => ImageFormat.Png;
#endif
    }
}