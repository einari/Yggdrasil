using System;
using System.Reflection;

namespace Yggdrasil.Binding
{
	public class DefaultBindingConvention : IBindingConvention
	{
        IContainer _container;

        public DefaultBindingConvention(IContainer container)
        {
            _container = container;
        }


		public bool CanBeBound(Type type)
		{
			if(type.Name.StartsWith("I") && _container.TypeSystem.GetDefinitionFor(type).IsInterface)
			{
				var targetType = GetTargetType(type);
				return targetType != null;
			}
			return false;
		}

		Type GetTargetType(Type type)
		{
            var typeDefinition = _container.TypeSystem.GetDefinitionFor(type);
            var typeName = typeDefinition.Namespace + "." + type.Name.Substring(1);
			return _container.TypeSystem.GetType(typeName);
		}

		public Type GetBindingTarget(Type type)
		{
			return GetTargetType(type);
		}
	}
}