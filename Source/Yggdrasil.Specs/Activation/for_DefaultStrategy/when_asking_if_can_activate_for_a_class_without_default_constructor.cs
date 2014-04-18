using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy
{
	public class when_asking_if_can_activate_for_a_class_without_default_constructor : given.a_default_activation_strategy
	{
		static bool result;

        Establish context = () =>
        {
            type_definition_mock.SetupGet(t => t.IsValueType).Returns(false);
            type_definition_mock.SetupGet(t => t.HasDefaultConstructor).Returns(false);
        };

		Because of = () => result = strategy.CanActivate(typeof(ClassWithIntDependencyOnConstructor));

		It should_result_in_false = () => result.ShouldBeFalse();
	}
}