using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy
{
	public class when_asking_if_struct_can_be_activated : given.a_complex_strategy
	{
		static bool result;

        Establish context = () => type_definition_mock.SetupGet(t => t.IsValueType).Returns(true);

		Because of = () => result = complex_strategy.CanActivate(typeof (SimpleStruct));

		It should_result_in_false = () => result.ShouldBeFalse();
	}
}
