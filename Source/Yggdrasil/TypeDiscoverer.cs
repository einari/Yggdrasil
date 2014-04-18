#region License

//
// Author: Einar Ingebrigtsen <einar@dolittle.com>
// Copyright (c) 2007-2011, DoLittle Studios
//
// Licensed under the Microsoft Permissive License (Ms-PL), Version 1.1 (the "License")
// you may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://Yggdrasil.codeplex.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion

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

		void CollectTypes()
		{
            foreach (var assembly in _assemblyLocator.GetAll())
#if(NETMF)
                _types.AddRange(assembly.GetTypes());
#else
                _types.AddRange(assembly.DefinedTypes.Select(t => t.AsType()));
#endif
		}


		private Type[] Find(Type type)
		{
            var typeInfo = type.GetTypeInfo();
			var query = from t in _types
						where typeInfo.IsAssignableFrom(t.GetTypeInfo()) && !t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsAbstract
						select t;
			var typesFound = query.ToArray();
			return typesFound;
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


        void ThrowIfMoreThanOneTypeFound(Type type, Type[] typesFound)
        {
            if (typesFound.Length > 1)
                throw new ArgumentException("More than one type found for '" + type.FullName + "'");
        }
	}
}
