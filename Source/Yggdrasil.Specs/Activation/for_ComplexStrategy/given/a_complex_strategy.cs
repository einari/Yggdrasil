using Yggdrasil;
using Yggdrasil.Activation;
using Machine.Specifications;
using Moq;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy.given
{
	public class a_complex_strategy
	{
		protected static ComplexStrategy complex_strategy;
		protected static Mock<IContainer> container_mock;

		Establish context = () =>
		                    	{
		                    		container_mock = new Mock<IContainer>();
		                    		complex_strategy = new ComplexStrategy(container_mock.Object);
		                    	};
	}
}
