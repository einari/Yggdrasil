using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.for_Container
{
	public class when_getting_an_instance_that_has_no_registration_but_can_be_discovered : given.a_container
	{
		static Mock<IBinding> binding_mock;
		static Mock<IStrategy> activation_strategy_mock;
		static IServiceWithImplementation instance;
		static ServiceWithImplementation expected_instance;

		Establish context = () =>
		                    	{
									expected_instance = new ServiceWithImplementation();
									activation_strategy_mock = new Mock<IStrategy>();
									activation_strategy_mock.Setup(a=>a.GetInstance(typeof(ServiceWithImplementation))).Returns(expected_instance);

									binding_mock = new Mock<IBinding>();
		                    		binding_mock.Setup(b => b.Service).Returns(typeof (IServiceWithImplementation));
		                    		binding_mock.Setup(b => b.Target).Returns(typeof (ServiceWithImplementation));
		                    		binding_mock.Setup(b => b.Strategy).Returns(activation_strategy_mock.Object);

		                    		binding_discoverer_mock.Setup(b => b.Discover(typeof (IServiceWithImplementation))).Returns(binding_mock.Object);
		                    	};

		Because of = () => instance = container.Get<IServiceWithImplementation>();

		It should_try_to_discover_binding = () => binding_discoverer_mock.Verify(b=>b.Discover(typeof(IServiceWithImplementation)));
		It should_forward_discovery_to_binding_manager = () => binding_manager_mock.Verify(b => b.Register(binding_mock.Object));
		It should_not_return_null = () => instance.ShouldNotBeNull();
		It should_return_expected_instance = () => instance.ShouldEqual(expected_instance);
	}
}