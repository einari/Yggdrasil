
namespace System.Reflection
{
    public class Activator
    {
        public static object CreateInstance(Type dataType)
        {
            var info = dataType.GetConstructor(new Type[0]);

            if (info == null)
                throw new NotSupportedException("The type does not have a constructor without any parameters");

            if (!info.IsPublic)
                throw new Exception("The type does not have a public constructor without any parameters");

            return info.Invoke(new object[0]);
        }

        public static object CreateInstance(Type dataType, params object[] args)
        {
            Type[] types = new Type[args.Length];
            for (var i = 0; i < args.Length; i++) types[i] = args[i].GetType();

            var info = dataType.GetConstructor(types);

            if (info == null)
                throw new NotSupportedException("The type does not have a constructor without any parameters");

            if (!info.IsPublic)
                throw new Exception("The type does not have a public constructor without any parameters");

            return info.Invoke(args);
        }

    }
}
