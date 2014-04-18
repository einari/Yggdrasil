using System;

namespace Yggdrasil.Types
{
    public class TypeSystem : ITypeSystem
    {
        public ITypeDefinition GetDefinitionFor(Type type)
        {
            throw new NotImplementedException();
        }


        public Type GetType(string fullName)
        {
            throw new NotImplementedException();
        }
    }
}
