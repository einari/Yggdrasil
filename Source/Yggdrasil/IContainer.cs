using System;
using Yggdrasil.Activation;
using Yggdrasil.Types;

namespace Yggdrasil
{
	public interface IContainer
	{
        ITypeSystem TypeSystem { get; }

#if(!NETMF)
		T Get<T>();
		void Register<TS, TT>(IScope scope=null) where TT : TS;
		void Register<TS>(TS instance, IScope scope = null);
#endif
		object Get(Type service);
		void Register(Type service, Type target, IScope scope = null);
		void Register(Type service, object instance, IScope scope=null);
	}
}
