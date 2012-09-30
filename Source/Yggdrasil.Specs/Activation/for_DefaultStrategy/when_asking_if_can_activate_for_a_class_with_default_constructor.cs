using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy
{
	public class when_asking_if_can_activate_for_a_class_with_default_constructor : given.a_default_activation_strategy
	{
		static bool result;

		Because of = () => result = strategy.CanActivate(typeof (ClassWithDefaultConstructor));

		It should_result_in_true = () => result.ShouldBeTrue();
	}
}
