namespace GarbageDb {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source)
                action(item);
        }

        public static IEnumerable<T> Tap<T>(this IEnumerable<T> source, Action<T> action) => source
            .Select(x => { action(x); return x; });
    }
}
