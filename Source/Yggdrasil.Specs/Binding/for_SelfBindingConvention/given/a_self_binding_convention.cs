using Yggdrasil.Binding;
using Machine.Specifications;

namespace Yggdrasil.Specs.Binding.for_SelfBindingConvention.given
{
	public class a_self_binding_convention
	{
		protected static SelfBindingConvention convention;
		Establish context = () => convention = new SelfBindingConvention();
	}
}
