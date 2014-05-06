using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Yggdrasil
{
	[Singleton]
	public class TypeDiscoverer : ITypeDiscoverer
	{
		readonly IAssemblyLocator _assemblyLocator;
		private readonly List<Type> _types;

		public TypeDiscoverer(IAssemblyLocator assemblyLocator)
		{
			_assemblyLocator = assemblyLocator;
			_types = new List<Type>();
			CollectTypes();
		}

        public Type[] FindAnyByName(string name)
        {
            var types = _types.Where(t => t.Name == name).ToArray();
            return types;
        }

        public Type FindByFullName(string name)
        {
            var type = _types.Where(t => t.FullName == name).SingleOrDefault();
            return type;
        }

		public Type FindSingle(Type type)
		{
			var typesFound = Find(type);

            ThrowIfMoreThanOneTypeFound(type, typesFound);
			return typesFound.SingleOrDefault();
		}

		public Type[] FindMultiple(Type type)
		{
			var typesFound = Find(type);
			return typesFound;
		}


        void CollectTypes()
        {
            foreach (var assembly in _assemblyLocator.GetAll())
                _types.AddRange(assembly.DefinedTypes.Select(t => t.AsType()));
        }


        Type[] Find(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var query = from t in _types
                        where typeInfo.IsAssignableFrom(t.GetTypeInfo()) && !t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsAbstract
                        select t;
            var typesFound = query.ToArray();
            return typesFound;
        }


        void ThrowIfMoreThanOneTypeFound(Type type, Type[] typesFound)
        {
            if (typesFound.Length > 1)
                throw new ArgumentException("More than one type found for '" + type.FullName + "'");
        }
    }
}
