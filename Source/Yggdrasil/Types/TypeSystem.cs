using System;
using System.Collections.Generic;

namespace Yggdrasil.Types
{
    public class TypeSystem : ITypeSystem
    {
        Dictionary<Type, TypeDefinition> _typeDefinitions = new Dictionary<Type, TypeDefinition>();

        ITypeDiscoverer _typeDiscoverer;

        public TypeSystem(ITypeDiscoverer typeDiscoverer)
        {
            _typeDiscoverer = typeDiscoverer;
        }


        public ITypeDefinition GetDefinitionFor(Type type)
        {
            if (_typeDefinitions.ContainsKey(type)) return _typeDefinitions[type];
            var typeDefinition = new TypeDefinition(type);
            _typeDefinitions[type] = typeDefinition;
            return typeDefinition;
        }


        public Type GetType(string fullName)
        {
            return _typeDiscoverer.FindByName(fullName);
        }
    }
}
