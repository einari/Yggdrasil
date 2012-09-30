using Yggdrasil.Activation;
using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_ConstantStrategy
{
	public class when_asking_for_instance
	{
		static ConstantStrategy strategy;
		static object instance;
		static object result;

		Establish context = () =>
		                    	{
		                    		instance = new object();
		                    		strategy = new ConstantStrategy(instance);
		                    	};

		Because of = () => result = strategy.GetInstance(typeof (object));

		It should_return_the_given_instance = () => result.ShouldEqual(instance);
	}
}
