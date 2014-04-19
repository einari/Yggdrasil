using System;
using Microsoft.SPOT;

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
            var container = Yggdrasil.ContainerContext.Current;
            var something = container.Get(typeof(ISomething)) as Something;


            Debug.Print(
                Resources.GetString(Resources.StringResources.String1));
        }

    }
}
