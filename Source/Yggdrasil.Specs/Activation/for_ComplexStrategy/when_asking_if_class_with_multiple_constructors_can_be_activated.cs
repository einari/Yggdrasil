using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy
{
	public class when_asking_if_class_with_multiple_constructors_can_be_activated : given.a_complex_strategy
	{
		static bool result;

		Because of = () => result = complex_strategy.CanActivate(typeof(ClassWithMultipleConstructors));

		It should_result_in_false = () => result.ShouldBeFalse();
	}
}