using System;

namespace Yggdrasil.Types
{
    public interface ITypeDefinition
    {
        Type Type { get; }

        string Namespace { get; }

        bool IsValueType { get; }
        bool IsInterface { get; }

        int ConstructorCount { get; }
        bool HasConstructor { get; }
        bool HasDefaultConstructor { get; }
        bool HasSingletonAttribute { get; }
        bool HasConstructorWithParameterTypes(params Type[] parameterTypes);
        bool HasConstructorParametersValueTypes { get; }

        Type[] GetParameterTypesForFirstConstructor();

        object CreateInstance();
        object CreateInstance(params object[] parameters);
    }
}
