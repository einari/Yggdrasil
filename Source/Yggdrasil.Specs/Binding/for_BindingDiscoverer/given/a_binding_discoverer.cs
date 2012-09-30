using Yggdrasil;
using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;

namespace Yggdrasil.Specs.Binding.for_BindingDiscoverer.given
{
	public class a_binding_discoverer
	{
		protected static BindingDiscoverer discoverer;
		protected static Mock<IActivationManager> activation_manager_mock;
		protected static Mock<ITypeDiscoverer> type_discoverer_mock;

		Establish context = () =>
		                    	{
		                    		activation_manager_mock = new Mock<IActivationManager>();
									type_discoverer_mock = new Mock<ITypeDiscoverer>();
		                    		discoverer = new BindingDiscoverer(activation_manager_mock.Object, type_discoverer_mock.Object);
		                    	};
	}
}
