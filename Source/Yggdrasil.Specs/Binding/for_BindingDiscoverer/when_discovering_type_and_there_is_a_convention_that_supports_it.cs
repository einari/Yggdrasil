using Yggdrasil.Binding;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.Binding.for_BindingDiscoverer
{
	public class when_discovering_type_and_there_is_a_convention_that_supports_it : given.a_binding_discoverer_with_one_convention
	{
		static IBinding binding;

		Establish context = () =>
		                    	{
									binding_convention_mock.Setup(b => b.CanBeBound(typeof(IServiceWithImplementation))).Returns(true);
									binding_convention_mock.Setup(b => b.GetBindingTarget(typeof(IServiceWithImplementation))).Returns(typeof(ServiceWithImplementation));
		                    	};

		Because of = () => binding = discoverer.Discover(typeof(IServiceWithImplementation));

		It should_discover_a_binding = () => binding.ShouldNotBeNull();
		It should_get_activation_strategy_for_target = () => activation_manager_mock.Verify(a => a.GetStrategyFor(typeof (ServiceWithImplementation)));
	}
}