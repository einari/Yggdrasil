using System;
using System.Reflection;
using Microsoft.SPOT;
using Yggdrasil.Types;

namespace Yggdrasil.Microframework.TestApp
{
    public interface ISomething
    {
    }

    public class Something
    {

    }


    public class Program
    {
        public static void Main()
        {

            
            //var type = typeof(Yggdrasil.ContainerContext).Assembly.GetType("Yggdrasil._TypeMetaData");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.Name.StartsWith("_"))
                    {
                        Debug.Print("Name : " + type.Name);
                    }
                }
            }


            //var container = Yggdrasil.ContainerContext.Current;
            //var something = container.Get(typeof(ISomething)) as Something;


            Debug.Print(
                Resources.GetString(Resources.StringResources.String1));
        }

    }
}
