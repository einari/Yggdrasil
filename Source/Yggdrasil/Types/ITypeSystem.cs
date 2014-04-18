using System;
namespace Yggdrasil.Types
{
    public interface ITypeSystem
    {
        ITypeDefinition GetDefinitionFor(Type type);
        Type GetType(string fullName);
    }
}
