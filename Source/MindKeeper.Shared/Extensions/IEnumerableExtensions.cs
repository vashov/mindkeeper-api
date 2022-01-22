using System.Collections.Generic;
using System.Linq;

namespace MindKeeper.Shared.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool HasValues<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }
    }
}
