using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy
{
	public class when_asking_if_class_with_one_constructor_with_dependencies_can_be_activated : given.a_complex_strategy
	{
		static bool result;

		Because of = () => result = complex_strategy.CanActivate(typeof (ClassWithServiceDependency));

		It should_result_in_true = () => result.ShouldBeTrue();
	}
}
