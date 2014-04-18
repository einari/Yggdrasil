using System;
using System.Reflection;
using Yggdrasil.Types;

namespace Yggdrasil.Binding
{
	public class SelfBindingConvention : IBindingConvention
	{
        IContainer _container;

        public SelfBindingConvention(IContainer container)
        {
            _container = container;
        }

		public bool CanBeBound(Type type)
		{
            return !_container.TypeSystem.GetDefinitionFor(type).IsInterface;
		}

		public Type GetBindingTarget(Type type)
		{
			return type;
		}
	}
}