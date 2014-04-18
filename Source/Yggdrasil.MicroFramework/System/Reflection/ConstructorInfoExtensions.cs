namespace System.Reflection
{
    
    public class ParameterInfo 
    {
        public Type ParameterType { get; private set; }
    }

    public static class ConstructorInfoExtensions
    {
        public static ParameterInfo[] GetParameters(this ConstructorInfo constructorInfo)
        {
            throw new NotImplementedException();
        }
    }
}
