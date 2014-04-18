using Machine.Specifications;

namespace Yggdrasil.Specs.Binding.for_DefaultBindingConvention
{
	public class when_asking_if_can_be_bound_for_interface_with_implementation_in_same_namespace : given.a_default_binding_convention
	{
		static bool result;

        Establish context = () =>
        {
            type_definition_mock.Setup(t => t.IsInterface).Returns(true);
            type_definition_mock.Setup(t => t.Namespace).Returns("SomeNamespace");
            type_system_mock.Setup(t => t.GetType("SomeNamespace.ServiceWithImplementation")).Returns(typeof(ServiceWithImplementation));
        };

		Because of = () => result = convention.CanBeBound(typeof(IServiceWithImplementation));

		It should_result_in_true = () => result.ShouldBeTrue();
	}
}