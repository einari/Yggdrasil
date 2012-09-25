using System;
using Yggdrasil.Execution.Activation;

namespace Yggdrasil.Execution.Binding
{
	public interface IBinding
	{
		Type Service { get; }
		Type Target { get; }
		IStrategy Strategy { get; }
		IScope Scope { get; set; }
	}
}