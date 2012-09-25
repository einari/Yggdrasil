using System;

namespace Yggdrasil.Binding
{
	public interface IBindingDiscoverer
	{
		void AddConvention(IBindingConvention convention);
		IBinding Discover(Type type);
	}
}
