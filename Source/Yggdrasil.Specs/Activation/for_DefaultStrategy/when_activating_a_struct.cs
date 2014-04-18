using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy
{
	public class when_activating_a_struct : given.a_default_activation_strategy
	{
		Because of = () => strategy.GetInstance(typeof(SimpleStruct));

        It should_create_instance = () => type_definition_mock.Verify(t => t.CreateInstance(), Moq.Times.Once());
	}
}