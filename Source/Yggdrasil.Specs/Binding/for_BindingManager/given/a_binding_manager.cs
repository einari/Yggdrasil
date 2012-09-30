using Yggdrasil.Binding;
using Machine.Specifications;

namespace Yggdrasil.Specs.Binding.for_BindingManager.given
{
	public class a_binding_manager
	{
		protected static BindingManager binding_manager;

		Establish context = () => binding_manager = new BindingManager();
	}
}
