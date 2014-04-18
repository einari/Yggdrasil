using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T item)
        {
            var index = 0;
            foreach (var itemInList in enumerable)
            {
                if (itemInList.Equals(item)) return index;
                index++;
            }

            return -1;
        }


        public static T[] ToArray<T>(this IEnumerable<T> enumerable)
        {
            var array = new T[enumerable.Count()];

            var index = 0;
            foreach (var item in enumerable)
            {
                array[index] = item;
                index++;
            }

            return array;
        }
    }
}
