using System;
using Machine.Specifications;

namespace Yggdrasil.Specs.Binding.for_DefaultBindingConvention
{
	public class when_getting_binding_target_for_interface_with_implementation_in_same_namespace : given.a_default_binding_convention
	{
		static Type target_type;

        Establish context = () =>
        {
            type_definition_mock.Setup(t => t.Namespace).Returns("SomeNamespace");
            type_system_mock.Setup(t => t.GetType("SomeNamespace.ServiceWithImplementation")).Returns(typeof(ServiceWithImplementation));
        };

		Because of = () => target_type = convention.GetBindingTarget(typeof(IServiceWithImplementation));

		It should_not_be_null = () => target_type.ShouldNotBeNull();
		It should_return_expected_implementation = () => target_type.ShouldEqual(typeof (ServiceWithImplementation));
	}
}