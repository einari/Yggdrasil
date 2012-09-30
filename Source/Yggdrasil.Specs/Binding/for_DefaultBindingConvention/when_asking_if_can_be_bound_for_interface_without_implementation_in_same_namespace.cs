using Machine.Specifications;

namespace Yggdrasil.Specs.Binding.for_DefaultBindingConvention
{
	public class when_asking_if_can_be_bound_for_interface_without_implementation_in_same_namespace : given.a_default_binding_convention
	{
		static bool result;

		Because of = () => result = convention.CanBeBound(typeof(IServiceWithoutImplementation));

		It should_result_in_false = () => result.ShouldBeFalse();
	}
}
