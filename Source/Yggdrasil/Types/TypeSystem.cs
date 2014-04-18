using System;
using System.Collections.Generic;

namespace Yggdrasil.Types
{
    public class TypeSystem : ITypeSystem
    {
        Dictionary<Type, TypeDefinition> _typeDefinitions = new Dictionary<Type, TypeDefinition>();

        public ITypeDefinition GetDefinitionFor(Type type)
        {
            if (_typeDefinitions.ContainsKey(type)) return _typeDefinitions[type];
            var typeDefinition = new TypeDefinition(type);
            _typeDefinitions[type] = typeDefinition;
            return typeDefinition;
        }


        public Type GetType(string fullName)
        {
            return Type.GetType(fullName);
        }
    }
}
