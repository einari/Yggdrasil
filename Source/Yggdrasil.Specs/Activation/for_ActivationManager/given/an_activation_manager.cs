using Yggdrasil;
using Yggdrasil.Activation;
using Machine.Specifications;
using Moq;

namespace Yggdrasil.Specs.Activation.for_ActivationManager.given
{
	public class an_activation_manager
	{
		protected static ActivationManager manager;
		protected static Mock<ITypeDiscoverer> type_discoverer_mock;
		protected static Mock<IStrategyActivator> strategy_activator_mock;

		Establish context = () =>
		                    	{
									type_discoverer_mock = new Mock<ITypeDiscoverer>();
									strategy_activator_mock = new Mock<IStrategyActivator>();
		                    		manager = new ActivationManager(type_discoverer_mock.Object, strategy_activator_mock.Object);
		                    	};
	}
}
