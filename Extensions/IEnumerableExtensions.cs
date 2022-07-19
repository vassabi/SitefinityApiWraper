using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Services.Extensions
{
    public enum OrderDirection
    {
        Ascending,
        Descending
    }

    public static class IEnumerableExtensions
    {
        public static void While<T>(this IEnumerable<T> collection, Action<T> operation)
        {
            collection.While(null, operation);
        }

        public static void While<T>(this IEnumerable<T> collection, Action<T, int> operation)
        {
            int index = 0;

            if (collection != null)
            {
                collection.ToList()
                    .While
                    (
                        (i) =>
                        {
                            operation.Invoke(i, index);
                            index++;
                        }
                    );
            }
        }

        public static void While<T>(this IEnumerable<T> collection, Predicate<T> condition, Action<T> operation)
        {
            collection.While(condition, operation, false);
        }

        public static void While<T>(this IEnumerable<T> collection, Predicate<T> condition, Action<T> operation, bool repeatUntilConditionMet)
        {
            IEnumerator<T> enumerator;
            T last;

            if (collection != null && collection.Count() > 0)
            {
                enumerator = collection.GetEnumerator();
                last = collection.LastOrDefault();

                while (enumerator.MoveNext())
                {
                    if (condition == null || condition.Invoke(enumerator.Current))
                    {
                        operation.Invoke(enumerator.Current);

                        if (last != null && enumerator.Current.Equals(last) && repeatUntilConditionMet)
                        {
                            collection.While(condition, operation, repeatUntilConditionMet);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}