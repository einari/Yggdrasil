using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.for_Container
{
	public class when_getting_an_instance_and_there_is_a_binding : given.a_container
	{
		static ClassWithDefaultConstructor expected_instance;
		static ClassWithDefaultConstructor result;
		static StandardBinding binding;
		static Mock<IStrategy> activation_strategy_mock;
		

		Establish context = () =>
		                    	{
		                    		expected_instance = new ClassWithDefaultConstructor();
									activation_strategy_mock = new Mock<IStrategy>();
									activation_strategy_mock.Setup(a => a.GetInstance(typeof(ClassWithDefaultConstructor))).Returns(expected_instance);
		                    		binding = new StandardBinding(typeof (ClassWithDefaultConstructor),
		                    		                              typeof (ClassWithDefaultConstructor),
		                    		                              activation_strategy_mock.Object);
		                    		binding_manager_mock.Setup(b => b.HasBinding(typeof (ClassWithDefaultConstructor))).Returns(true);
									binding_manager_mock.Setup(b => b.GetBinding(typeof(ClassWithDefaultConstructor))).Returns(binding);
		                    	};

		Because of = () => result = container.Get<ClassWithDefaultConstructor>();

		It should_not_return_null = () => result.ShouldNotBeNull();
		It should_return_expected_instance = () => result.ShouldEqual(expected_instance);
	}
}
