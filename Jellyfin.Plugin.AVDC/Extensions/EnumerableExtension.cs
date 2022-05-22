using System.Collections.Generic;
using System.Linq;

namespace Jellyfin.Plugin.AVDC.Extensions;

internal static class EnumerableExtension
{
    public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (index, item));
    }
}