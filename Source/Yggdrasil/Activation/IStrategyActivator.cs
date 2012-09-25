using System;

namespace Yggdrasil.Execution.Activation
{
	public interface IStrategyActivator
	{
		IStrategy GetInstance(Type type);
	}
}