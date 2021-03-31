#if __EMBY__

using System;
using MediaBrowser.Model.Logging;

namespace Jellyfin.Plugin.AVDC.Extensions
{
    public static class LogManagerExtension
    {
        public static ILogger CreateLogger<T>(this ILogManager factory)
        {
            return factory.GetLogger(typeof(T).FullName);
        }

        public static ILogger CreateLogger(this ILogManager factory, Type type)
        {
            return factory.GetLogger(type.FullName);
        }
    }
}

#endif