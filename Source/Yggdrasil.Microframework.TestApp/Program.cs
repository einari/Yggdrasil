using System;
using Microsoft.SPOT;

namespace Yggdrasil.Microframework.TestApp
{
    public interface ISomething
    {
    }

    public class TypeInfo
    {
        public Type Type;
        public bool IsValueType;
        public bool IsInterface;
        public int ConstructorCount;
    }

    public class Something
    {
        public static Type[] Hello;
        public static TypeInfo[] Types;

        static Something()
        {
            Hello = new[] {
                typeof(ISomething),
                typeof(Something)
            };


            Types = new [] {
                new TypeInfo { Type = typeof(ISomething), IsValueType = true, IsInterface = true, ConstructorCount = 0 },
                new TypeInfo { Type = typeof(Something), IsValueType = false, IsInterface = false, ConstructorCount = 3 },
                new TypeInfo { Type = typeof(ISomething), IsValueType = true, IsInterface = true, ConstructorCount = 0 },
                new TypeInfo { Type = typeof(Something), IsValueType = false, IsInterface = false, ConstructorCount = 3 },
                new TypeInfo { Type = typeof(ISomething), IsValueType = true, IsInterface = true, ConstructorCount = 0 },
                new TypeInfo { Type = typeof(Something), IsValueType = false, IsInterface = false, ConstructorCount = 3 }

            };

            
        }

    }


    public class Program
    {
        public static void Main()
        {
            var container = Yggdrasil.ContainerContext.Current;
            var something = container.Get(typeof(ISomething)) as Something;


            Debug.Print(
                Resources.GetString(Resources.StringResources.String1));
        }

    }
}
