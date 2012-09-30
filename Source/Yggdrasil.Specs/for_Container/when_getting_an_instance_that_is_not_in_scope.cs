using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.for_Container
{
	public class when_getting_an_instance_that_is_not_in_scope : given.a_container
	{
		static Mock<IScope> scope_mock;
		static Mock<IBinding> binding_mock;
		static Mock<IStrategy> strategy_mock;
		static object expected_instance;
		

		Establish context = () =>
		                    	{
		                    		expected_instance = new object();
		                    		strategy_mock = new Mock<IStrategy>();
		                    		strategy_mock.Setup(s => s.GetInstance(typeof (object))).Returns(expected_instance);
		                    		scope_mock = new Mock<IScope>();
		                    		scope_mock.Setup(s => s.IsInScope(typeof (object))).Returns(false);
		                    		binding_mock = new Mock<IBinding>();
		                    		binding_mock.Setup(b => b.Target).Returns(typeof (object));
		                    		binding_mock.Setup(b => b.Scope).Returns(scope_mock.Object);
		                    		binding_mock.Setup(b => b.Strategy).Returns(strategy_mock.Object);
		                    		binding_manager_mock.Setup(b => b.HasBinding(typeof (object))).Returns(true);
		                    		binding_manager_mock.Setup(b => b.GetBinding(typeof (object))).Returns(binding_mock.Object);
		                    	};

		Because of = () => container.Get(typeof(object));

		It should_set_the_expected_instance_in_scope = () => scope_mock.Verify(s => s.SetInstance(typeof(object), expected_instance));
		
	}
}