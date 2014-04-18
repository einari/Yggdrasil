using System;
using Yggdrasil.Types;

namespace Yggdrasil.Activation
{
	public class StrategyActivator : IStrategyActivator
	{
		IContainer _container;

		public StrategyActivator(IContainer container)
		{
			_container = container;
		}

		public IStrategy GetInstance(Type type)
		{
            var typeDefinition = _container.TypeSystem.GetDefinitionFor(type);
            if (typeDefinition.HasDefaultConstructor) return typeDefinition.CreateInstance() as IStrategy;

            if (typeDefinition.HasConstructorWithParameterTypes(typeof(IContainer))) return typeDefinition.CreateInstance(_container) as IStrategy;

			//throw new ActivationStrategyConstructorMissing(type);
            return null;
		}
	}
}