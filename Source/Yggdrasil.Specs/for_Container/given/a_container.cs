using IOCContainer = Yggdrasil.Container;
using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using Yggdrasil.Types;

namespace Yggdrasil.Specs.for_Container.given
{
	public class a_container
	{
		protected static IOCContainer container;
		protected static Mock<IBindingDiscoverer> binding_discoverer_mock;
		protected static Mock<IBindingManager> binding_manager_mock;
		protected static Mock<IActivationManager> activation_manager_mock;
        protected static Mock<ITypeSystem> type_system_mock;

		Establish context = () =>
		                    	{
									binding_discoverer_mock = new Mock<IBindingDiscoverer>();
									binding_manager_mock = new Mock<IBindingManager>();
									activation_manager_mock = new Mock<IActivationManager>();
                                    type_system_mock = new Mock<ITypeSystem>();
									container = new IOCContainer(binding_manager_mock.Object, binding_discoverer_mock.Object, activation_manager_mock.Object, type_system_mock.Object);
		                    	};
	}
}
