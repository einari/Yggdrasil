using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy
{
	public class when_activating_a_struct : given.a_default_activation_strategy
	{
		static SimpleStruct? result;

		Because of = () => result = (SimpleStruct)strategy.GetInstance(typeof(SimpleStruct));

		It should_create_instance = () => result.ShouldNotBeNull();
	}
}