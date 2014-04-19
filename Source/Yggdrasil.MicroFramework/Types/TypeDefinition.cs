using System;
using Microsoft.SPOT;

namespace Yggdrasil.Types
{
    public class TypeDefinition : ITypeDefinition
    {
        public Type Type { get; private set; }
        public string Namespace { get; private set; }
        public bool IsValueType { get; private set; }
        public bool IsInterface { get { return Type.IsInterface; } }
        public int ConstructorCount { get; private set; }
        public bool HasConstructor { get; private set; }
        public bool HasDefaultConstructor { get; private set; }
        public bool HasSingletonAttribute { get; private set; }
        public bool HasConstructorParametersValueTypes { get; private set; }

        public bool HasConstructorWithParameterTypes(params Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public Type[] GetParameterTypesForFirstConstructor()
        {
            throw new NotImplementedException();
        }

        public object CreateInstance()
        {
            var instance = Type.GetConstructor(new Type[0]).Invoke(new object[0]);
            return instance;
        }

        public object CreateInstance(params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
