using System;

namespace Yggdrasil.Binding
{
	public interface IBindingConvention
	{
		bool CanBeBound(Type type);
		Type GetBindingTarget(Type type);
	}
}