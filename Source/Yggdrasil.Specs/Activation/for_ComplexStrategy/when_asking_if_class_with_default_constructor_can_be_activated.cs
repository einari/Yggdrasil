using Machine.Specifications;
using Moq;
using Yggdrasil.Types;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy
{
	public class when_asking_if_class_with_default_constructor_can_be_activated : given.a_complex_strategy
	{
		static bool result;

        Establish context = () => type_definition_mock.SetupGet(t => t.HasDefaultConstructor).Returns(true);

		Because of = () => result = complex_strategy.CanActivate(typeof (ClassWithDefaultConstructor));

		It should_result_in_false = () => result.ShouldBeFalse();
	}
}
