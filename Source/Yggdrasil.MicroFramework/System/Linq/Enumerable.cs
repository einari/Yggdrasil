using System.Collections.Generic;

namespace System.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<T> Select<T>(this IEnumerable<T> enumerable, Func<T, object> predicate)
        {
            throw new NotImplementedException();
        }


        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }


        public static int Count<T>(this IEnumerable<T> enumerable)
        {
            var count = 0;
            foreach (var item in enumerable) count++;
            return count;
        }

        public static T First<T>(this IEnumerable<T> enumerable)
        {
            throw new NotImplementedException();
        }


        public static bool Any<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> enumerable)
        {
            throw new NotImplementedException();
        }


        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable)
        {
            throw new NotImplementedException();
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public static List<T> ToList<T>(this IEnumerable<T> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}
