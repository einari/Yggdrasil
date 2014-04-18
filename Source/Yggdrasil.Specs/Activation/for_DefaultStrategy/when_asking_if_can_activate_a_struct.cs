using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy
{
	public class when_asking_if_can_activate_a_struct : given.a_default_activation_strategy
	{
		static bool result;

        Establish context = () =>
        {
            type_definition_mock.SetupGet(t => t.IsValueType).Returns(true);
            type_definition_mock.SetupGet(t => t.HasDefaultConstructor).Returns(false);
        };

		Because of = () => result = strategy.CanActivate(typeof(SimpleStruct));

		It should_result_in_true = () => result.ShouldBeTrue();
	}
}