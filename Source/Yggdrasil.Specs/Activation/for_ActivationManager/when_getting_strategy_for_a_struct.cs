using Yggdrasil.Activation;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Yggdrasil.Specs.Activation.for_ActivationManager
{
	public class when_getting_strategy_for_a_struct_and_there_is_a_strategy_for_it : given.an_activation_manager
	{
		static IStrategy strategy;
		static Mock<IStrategy> activation_strategy_mock;

		Establish context = () =>
		                    	{
		                    		activation_strategy_mock = new Mock<IStrategy>();
		                    		activation_strategy_mock.Setup(a => a.CanActivate(typeof (SimpleStruct))).Returns(true);
									strategy_activator_mock.Setup(s => s.GetInstance(activation_strategy_mock.Object.GetType())).Returns(activation_strategy_mock.Object);
		                    		type_discoverer_mock.Setup(t => t.FindMultiple<IStrategy>()).Returns(new[] {activation_strategy_mock.Object.GetType()});
		                    	};

		Because of = () => strategy = manager.GetStrategyFor(typeof(SimpleStruct));

		It should_return_the_supported_strategy = () => strategy.ShouldEqual(activation_strategy_mock.Object);
	}
}