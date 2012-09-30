using Yggdrasil.Binding;
using Machine.Specifications;

namespace Yggdrasil.Specs.for_Container
{
	public class when_registering_a_service_with_a_target : given.a_container
	{
		static IBinding binding;

		Establish context = () => binding_manager_mock.Setup(b => b.Register(Moq.It.IsAny<IBinding>())).Callback((IBinding b) => binding = b);

		Because of = () => container.Register<IServiceWithImplementation, ServiceWithImplementation>();

		It should_ask_activation_manager_for_strategy_for_target = () => activation_manager_mock.Verify(a => a.GetStrategyFor(typeof(ServiceWithImplementation)));
		It should_forward_standard_binding_to_binding_manager = () => binding_manager_mock.Verify(b => b.Register(Moq.It.IsAny<StandardBinding>()));
		It should_forward_binding_with_correct_service = () => binding.Service.ShouldEqual(typeof(IServiceWithImplementation));
		It should_forward_binding_with_correct_target = () => binding.Target.ShouldEqual(typeof(ServiceWithImplementation));
	}
}