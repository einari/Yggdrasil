using Yggdrasil;
using Yggdrasil.Activation;
using Machine.Specifications;
using Moq;
using Yggdrasil.Types;
using System;

namespace Yggdrasil.Specs.Activation.for_ComplexStrategy.given
{
	public class a_complex_strategy
	{
		protected static ComplexStrategy complex_strategy;
		protected static Mock<IContainer> container_mock;
        protected static Mock<ITypeSystem> type_system_mock;
        protected static Mock<ITypeDefinition> type_definition_mock;

		Establish context = () =>
		                    	{
		                    		container_mock = new Mock<IContainer>();
                                    type_system_mock = new Mock<ITypeSystem>();
                                    container_mock.SetupGet(c => c.TypeSystem).Returns(type_system_mock.Object);
		                    		complex_strategy = new ComplexStrategy(container_mock.Object);

                                    type_definition_mock = new Mock<ITypeDefinition>();
                                    type_system_mock.Setup(t => t.GetDefinitionFor(Moq.It.IsAny<Type>())).Returns(type_definition_mock.Object);
		                    	};
	}
}
