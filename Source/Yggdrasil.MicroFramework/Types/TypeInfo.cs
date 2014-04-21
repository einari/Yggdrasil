using System;

namespace Yggdrasil.Types
{
    public class TypeInfo
    {
        public Type Type;
        public string Namespace;
        public ConstructorInfo[] Constructors;
        public bool HasSingletonAttribute;
    }
}
