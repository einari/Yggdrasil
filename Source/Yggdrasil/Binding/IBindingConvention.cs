using System;

namespace Yggdrasil.Execution.Binding
{
	public interface IBindingConvention
	{
		bool CanBeBound(Type type);
		Type GetBindingTarget(Type type);
	}
}