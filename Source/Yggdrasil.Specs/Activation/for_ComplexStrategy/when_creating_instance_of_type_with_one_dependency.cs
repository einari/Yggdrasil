using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy
{
	public class when_creating_instance_of_type_with_one_dependency : given.a_complex_strategy
	{
		static ClassWithServiceDependency instance;
        static ServiceWithImplementation service_instance;

        Establish context = () =>
        {
            type_definition_mock.SetupGet(t => t.IsValueType).Returns(false);
            type_definition_mock.SetupGet(t => t.HasDefaultConstructor).Returns(false);
            type_definition_mock.SetupGet(t => t.HasConstructor).Returns(true);
            type_definition_mock.SetupGet(t => t.ConstructorCount).Returns(1);
            type_definition_mock.SetupGet(t => t.HasConstructorParametersValueTypes).Returns(true);

            type_definition_mock.Setup(t => t.GetParameterTypesForFirstConstructor()).Returns(new [] { typeof(IServiceWithImplementation) });

            service_instance = new ServiceWithImplementation();

            container_mock.Setup(c => c.Get(typeof(IServiceWithImplementation))).Returns(service_instance);
        };

		Because of = () => instance = (ClassWithServiceDependency)complex_strategy.GetInstance(typeof (ClassWithServiceDependency));

        It should_create_instance_with_service_passed_as_parameter = () => type_definition_mock.Verify(t => t.CreateInstance(new object[] { service_instance }));
	}
}
