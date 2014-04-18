using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy
{
	public class when_asking_if_class_with_multiple_constructors_can_be_activated : given.a_complex_strategy
	{
		static bool result;

        Establish context = () =>
        {
            type_definition_mock.SetupGet(t => t.IsValueType).Returns(false);
            type_definition_mock.SetupGet(t => t.HasDefaultConstructor).Returns(false);
            type_definition_mock.SetupGet(t => t.HasConstructor).Returns(true);
            type_definition_mock.SetupGet(t => t.ConstructorCount).Returns(2);
            type_definition_mock.SetupGet(t => t.HasConstructorParametersValueTypes).Returns(true);
        };

		Because of = () => result = complex_strategy.CanActivate(typeof(ClassWithMultipleConstructors));

		It should_result_in_false = () => result.ShouldBeFalse();
	}
}