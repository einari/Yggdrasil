using Yggdrasil;
using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using Yggdrasil.Types;

namespace Yggdrasil.Specs.Binding.for_BindingDiscoverer.given
{
	public class a_binding_discoverer
	{
		protected static BindingDiscoverer discoverer;
        protected static Mock<IContainer> container_mock;
		protected static Mock<IActivationManager> activation_manager_mock;
        protected static Mock<ITypeSystem> type_system_mock;
		protected static Mock<ITypeDiscoverer> type_discoverer_mock;

		Establish context = () =>
		                    	{
                                    container_mock = new Mock<IContainer>();
		                    		activation_manager_mock = new Mock<IActivationManager>();
                                    type_system_mock = new Mock<ITypeSystem>();
									type_discoverer_mock = new Mock<ITypeDiscoverer>();
		                    		discoverer = new BindingDiscoverer(container_mock.Object, activation_manager_mock.Object, type_system_mock.Object, type_discoverer_mock.Object);
		                    	};
	}
}
