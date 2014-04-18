using System;
using System.Reflection;

namespace Yggdrasil.Binding
{
	public class DefaultBindingConvention : IBindingConvention
	{
		public bool CanBeBound(Type type)
		{
			if(type.Name.StartsWith("I") && type.GetTypeInfo().IsInterface)
			{
				var targetType = GetTargetType(type);
				return targetType != null;
			}
			return false;
		}

		static Type GetTargetType(Type type)
		{
#if(NETMF)
            return type; // throw new NotImplementedException();
#else
            var typeName = type.Namespace + "." + type.Name.Substring(1);
			return type.GetTypeInfo().Assembly.GetType(typeName);
#endif
		}

		public Type GetBindingTarget(Type type)
		{
			return GetTargetType(type);
		}
	}
}