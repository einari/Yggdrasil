using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;

namespace Yggdrasil.Specs.for_Container
{
	public class when_registering_a_constant : given.a_container
	{
		static object constant_to_register;
		static IBinding binding;

		Establish context = () =>
		                    	{
		                    		constant_to_register = new object();
		                    		binding_manager_mock.Setup(b => b.Register(Moq.It.IsAny<IBinding>())).Callback(
		                    			(IBinding b) => binding = b);
		                    	};

		Because of = () => container.Register(constant_to_register);

		It should_register_with_a_constant_strategy = () => binding.Strategy.ShouldBeOfType<ConstantStrategy>();
	}
}
