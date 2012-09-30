using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.Binding.for_BindingManager
{
	public class when_registering_class_marked_with_singleton_and_scope_is_not_defined : given.a_binding_manager
	{
		static Mock<IBinding> binding_mock;
		static IScope scope_set;

		Establish context = () =>
		                    	{
									binding_mock = new Mock<IBinding>();
		                    		binding_mock.Setup(b => b.Service).Returns(typeof (ISomething));
		                    		binding_mock.Setup(b => b.Target).Returns(typeof (TargetWithSingletonAttribute));
		                    		binding_mock.SetupSet(b => b.Scope).Callback(s => scope_set = s);
		                    	};

		Because of = () => binding_manager.Register(binding_mock.Object);

		It should_set_singleton_scope_on_binding = () => scope_set.ShouldBeOfType<SingletonScope>();
	}
}
