#if __EMBY__
using System;
using MediaBrowser.Model.Logging;

namespace Jellyfin.Plugin.AVDC.Extensions
{
    internal static class LogManagerExtension
    {
        public static ILogger CreateLogger<T>(this ILogManager factory)
        {
            return factory.GetLogger(Format(typeof(T)));
        }

        private static string Format(Type type)
        {
            return $"{Constants.Avdc}.{type.Name}";
        }
    }
}

#endif