using System;
using Yggdrasil.Activation;

namespace Yggdrasil
{
	public interface IContainer
	{
		T Get<T>();
		object Get(Type service);
		void Register<TS, TT>(IScope scope=null) where TT : TS;
		void Register(Type service, Type target, IScope scope = null);
		void Register<TS>(TS instance, IScope scope = null);
		void Register(Type service, object instance, IScope scope=null);
	}
}
