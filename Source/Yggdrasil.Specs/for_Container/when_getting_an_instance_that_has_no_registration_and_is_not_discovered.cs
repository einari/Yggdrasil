using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.for_Container
{
	public class when_getting_an_instance_that_has_no_registration_and_is_not_discovered : given.a_container
	{
		static IServiceWithImplementation instance;

		Because of = () => instance = container.Get<IServiceWithImplementation>();

		It should_forward_discovery_to_binding_manager = () => binding_manager_mock.Verify(b=>b.Register(Moq.It.IsAny<IBinding>()), Times.Never());
		It should_return_a_null_instance = () => instance.ShouldBeNull();
	}
}