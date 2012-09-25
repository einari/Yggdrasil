using System;

namespace Yggdrasil.Activation
{
	public interface IStrategy
	{
		bool CanActivate(Type type);
		object GetInstance(Type type);
	}
}