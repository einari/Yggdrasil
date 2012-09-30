using System;
using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.for_Container
{
	public class when_getting_an_instance_that_is_in_scope_return_that_instance : given.a_container
	{
		static Mock<IScope> scope_mock;
		static Mock<IBinding> binding_mock;
		static object expected_instance;
		static object result;

		Establish context = () =>
		                    	{
									expected_instance = new object();
									scope_mock = new Mock<IScope>();
		                    		scope_mock.Setup(s => s.IsInScope(typeof (object))).Returns(true);
		                    		scope_mock.Setup(s => s.GetInstance(typeof (object))).Returns(expected_instance);
									binding_mock = new Mock<IBinding>();
		                    		binding_mock.Setup(b => b.Target).Returns(typeof (object));
		                    		binding_mock.Setup(b => b.Scope).Returns(scope_mock.Object);
									binding_manager_mock.Setup(b => b.HasBinding(typeof(object))).Returns(true);
									binding_manager_mock.Setup(b => b.GetBinding(typeof(object))).Returns(binding_mock.Object);
		                    	};

		Because of = () => result = container.Get(typeof (object));


		It should_get_the_expected_instance = () => result.ShouldEqual(expected_instance);
	}
}
