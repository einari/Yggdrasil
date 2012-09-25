using System;
using System.Reflection;

namespace Yggdrasil.Binding
{
	public class SelfBindingConvention : IBindingConvention
	{
		public bool CanBeBound(Type type)
		{
            return !type.GetTypeInfo().IsInterface;
		}

		public Type GetBindingTarget(Type type)
		{
			return type;
		}
	}
}