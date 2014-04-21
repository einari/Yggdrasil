using System;
using Microsoft.SPOT;
using Yggdrasil.Types;

namespace Yggdrasil.Microframework.TestApp
{
    public interface ISomething
    {
    }


    public class Something
    {
        public static Type[] Hello;
        public static TypeInfo[] Types;
        public static ConstructorInfo[] c;

        static Something()
        {
            Hello = new[] {
                typeof(ISomething),
                typeof(Something)
            };

            c = new[] 
                    { 
                        new ConstructorInfo 
                        { 
                            Parameters = new [] 
                            { 
                                new ConstructorParameter 
                                { 
                                    Type = typeof(string), 
                                    Name = "blah" 
                                } 
                            } 
                        } 
                    }; 


            Types = new[] {
                new TypeInfo { 
                    Type = typeof(ISomething), 
                    HasSingletonAttribute = true, 
                    Constructors = new[] 
                    { 
                        new ConstructorInfo 
                        { 
                            Parameters = new [] 
                            { 
                                new ConstructorParameter 
                                { 
                                    Type = typeof(string), 
                                    Name = "blah" 
                                } 
                            } 
                        } 
                    } 
                },
                new TypeInfo { Type = typeof(Something), HasSingletonAttribute = true  },

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
