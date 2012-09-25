using System;

namespace Yggdrasil.Execution.Activation
{
	public class ActivationStrategyConstructorMissing : Exception
	{
		public ActivationStrategyConstructorMissing(Type type)
			: base("ActivationStrategy ('" + type.Name + "') does not have a recognized constructor")
		{

		}
	}
}