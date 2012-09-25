using System;
using Yggdrasil.Activation;

namespace Yggdrasil.Binding
{
	public interface IBinding
	{
		Type Service { get; }
		Type Target { get; }
		IStrategy Strategy { get; }
		IScope Scope { get; set; }
	}
}