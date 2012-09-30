using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy
{
	public class when_activating_a_class_with_default_constructor : given.a_default_activation_strategy
	{
		static ClassWithDefaultConstructor result;

		Because of = () => result = (ClassWithDefaultConstructor)strategy.GetInstance(typeof(ClassWithDefaultConstructor));

		It should_create_instance = () => result.ShouldNotBeNull();
	}
}