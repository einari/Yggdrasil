using System;

namespace Yggdrasil.Activation
{
	public interface IStrategyActivator
	{
		IStrategy GetInstance(Type type);
	}
}