using System;

namespace Yggdrasil.Execution.Binding
{
	public interface IBindingDiscoverer
	{
		void AddConvention(IBindingConvention convention);
		IBinding Discover(Type type);
	}
}
