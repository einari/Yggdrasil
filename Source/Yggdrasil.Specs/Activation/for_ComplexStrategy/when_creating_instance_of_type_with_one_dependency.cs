using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy
{
	public class when_creating_instance_of_type_with_one_dependency : given.a_complex_strategy
	{
		static ClassWithServiceDependency instance;

		Establish context = () => container_mock.Setup(c => c.Get(typeof(IServiceWithImplementation))).Returns(new ServiceWithImplementation());

		Because of = () => instance = (ClassWithServiceDependency)complex_strategy.GetInstance(typeof (ClassWithServiceDependency));

		It should_create_instance = () => instance.ShouldNotBeNull();
		It should_pass_in_an_instance_of_the_dependency = () => instance.Service.ShouldNotBeNull();
	}
}
