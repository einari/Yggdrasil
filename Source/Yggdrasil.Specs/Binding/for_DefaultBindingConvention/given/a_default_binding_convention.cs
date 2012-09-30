using Yggdrasil.Binding;
using Machine.Specifications;

namespace Yggdrasil.Specs.Binding.for_DefaultBindingConvention.given
{
	public class a_default_binding_convention
	{
		protected static DefaultBindingConvention convention;

		Establish context = () => convention = new DefaultBindingConvention();
	}
}
