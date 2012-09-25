using System;

namespace Yggdrasil.Activation
{
	public interface IActivationManager
	{
		IStrategy GetStrategyFor(Type type);
	}
}