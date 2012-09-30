using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;

namespace Yggdrasil.Specs.Binding.for_BindingDiscoverer.given
{
	public class a_binding_discoverer_with_one_convention : a_binding_discoverer
	{
		protected static Mock<IBindingConvention> binding_convention_mock;

		Establish context = () =>
		                    	{
		                    		binding_convention_mock = new Mock<IBindingConvention>();
		                    		discoverer.AddConvention(binding_convention_mock.Object);
		                    	};

	}
}