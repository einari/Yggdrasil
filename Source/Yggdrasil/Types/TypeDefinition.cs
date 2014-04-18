using System;
using System.Reflection;
using System.Linq;

namespace Yggdrasil.Types
{
    public class TypeDefinition : ITypeDefinition
    {
        TypeInfo    _typeInfo;


        public TypeDefinition(Type type)
        {
            Type = Type;
            _typeInfo = type.GetTypeInfo();
        }


        public Type Type { get; private set; }

        public string Namespace { get { return Type.Namespace; } }
        public bool IsValueType { get { return _typeInfo.IsValueType; } }
        public bool IsInterface { get { return _typeInfo.IsInterface; } }
        public int ConstructorCount { get { return _typeInfo.DeclaredConstructors.Count(); } }
        public bool HasConstructor { get { return ConstructorCount >= 1; } }

        public bool HasDefaultConstructor { get { return _typeInfo.DeclaredConstructors.Any(c => c.GetParameters().Length == 0); } }
        public bool HasSingletonAttribute { get { return _typeInfo.GetCustomAttributes(typeof(SingletonAttribute), true).Count() == 1; } }

        public bool HasConstructorParametersValueTypes { get { return _typeInfo.DeclaredConstructors.Where(c => c.GetParameters().Any(p => p.ParameterType.GetTypeInfo().IsValueType)).Count() >= 0; } }

        public bool HasConstructorWithParameterTypes(params Type[] parameterTypes)
        {
            return _typeInfo.DeclaredConstructors.Where(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes)).Count() == 1;
            
        }

        public object CreateInstance()
        {
            return Activator.CreateInstance(Type);
        }

        public object CreateInstance(object[] parameters)
        {
            return Activator.CreateInstance(Type, parameters);
        }
        

        public Type[] GetParameterTypesForFirstConstructor()
        {
            var parameterTypes = _typeInfo.DeclaredConstructors.First().GetParameters().Select(p => p.ParameterType).ToArray();
            return parameterTypes;
        }
    }
}
