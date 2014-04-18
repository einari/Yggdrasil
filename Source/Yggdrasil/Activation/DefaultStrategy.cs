using System;
using Yggdrasil.Types;

namespace Yggdrasil.Activation
{
	public class DefaultStrategy : IStrategy
	{
        IContainer _container;

        public DefaultStrategy(IContainer container)
        {
            _container = container;
        }


		public bool CanActivate(Type type)
		{
            var typeDefinition = _container.TypeSystem.GetDefinitionFor(type);
            if (typeDefinition.IsValueType) return true;

            if (typeDefinition.HasDefaultConstructor) return true;
            return false;
		}

		public object GetInstance(Type type)
		{
            var typeDefinition = _container.TypeSystem.GetDefinitionFor(type);
            return typeDefinition.CreateInstance();
		}
	}
}