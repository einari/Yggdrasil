using System;
using System.Reflection;
using Yggdrasil.Types;

namespace Yggdrasil.Activation
{
	public class ComplexStrategy : IStrategy
	{
		IContainer _container;

		public ComplexStrategy(IContainer container)
		{
			_container = container;
		}

		public bool CanActivate(Type type)
		{
            var typeDefinition = _container.TypeSystem.GetDefinitionFor(type);
			if (typeDefinition.IsValueType || typeDefinition.HasDefaultConstructor )
				return false;

            if (!typeDefinition.HasConstructor) return false;
            if (typeDefinition.ConstructorCount != 1) return false;

            if (typeDefinition.HasConstructorParametersValueTypes) return false;

			return true;
		}

		public object GetInstance(Type type)
		{
            var typeDefinition = _container.TypeSystem.GetDefinitionFor(type);
            
            var parameterTypes = typeDefinition.GetParameterTypesForFirstConstructor();
            var parameters = new object[parameterTypes.Length];
            for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                parameters[parameterIndex] = _container.Get(parameterTypes[parameterIndex]);

            var instance = typeDefinition.CreateInstance(parameters);

			return instance;
		}
	}
}
