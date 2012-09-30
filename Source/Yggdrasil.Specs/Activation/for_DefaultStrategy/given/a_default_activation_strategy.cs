using Yggdrasil.Activation;
using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy.given
{
	public class a_default_activation_strategy
	{
		protected static DefaultStrategy strategy;

		Establish context = () => strategy = new DefaultStrategy();
	}
}
