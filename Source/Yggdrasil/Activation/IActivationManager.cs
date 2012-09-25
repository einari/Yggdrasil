using System;

namespace Yggdrasil.Execution.Activation
{
	public interface IActivationManager
	{
		IStrategy GetStrategyFor(Type type);
	}
}