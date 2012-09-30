using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.Binding.for_BindingManager
{
	public class when_asking_for_binding_and_it_has_been_registered : given.a_binding_manager
	{
		static Mock<IBinding> binding_mock;
		static IBinding binding;

		Establish context = () =>
		                    	{
									binding_mock = new Mock<IBinding>();
		                    		binding_mock.Setup(b => b.Service).Returns(typeof (IServiceWithImplementation));
		                    		binding_mock.Setup(b => b.Target).Returns(typeof (ServiceWithImplementation));
									binding_manager.Register(binding_mock.Object);
		                    	};

		Because of = () => binding = binding_manager.GetBinding(typeof (IServiceWithImplementation));

		It should_return_the_binding_registered = () => binding.ShouldEqual(binding_mock.Object);
	}
}
