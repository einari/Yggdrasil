using System;

namespace Yggdrasil.Types
{
    public class TypeDefinition : ITypeDefinition
    {
        public TypeDefinition()
        {
        }


        public Type Type { get; private set; }

        public string Namespace { get; private set; }

        public bool IsValueType { get; set; }
        

        public bool HasDefaultConstructor { get; set; }

        public bool HasSingletonAttribute { get; set; }

        public bool HasConstructorWithParameterTypes(params Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public object CreateInstance()
        {
            throw new NotImplementedException();
        }

        public object CreateInstance(object[] parameters)
        {
            throw new NotImplementedException();
        }


        public bool IsInterface { get; private set; }
        public int ConstructorCount { get; private set; }
        public bool HasConstructor { get; private set; }

        public bool HasConstructorParametersValueTypes { get; private set; }

        public Type[] GetParameterTypesForFirstConstructor()
        {
            throw new NotImplementedException();
        }
    }
}
