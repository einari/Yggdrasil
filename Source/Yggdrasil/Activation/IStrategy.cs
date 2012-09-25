using System;

namespace Yggdrasil.Execution.Activation
{
	public interface IStrategy
	{
		bool CanActivate(Type type);
		object GetInstance(Type type);
	}
}