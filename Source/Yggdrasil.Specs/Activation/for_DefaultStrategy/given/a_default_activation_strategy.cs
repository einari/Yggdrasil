using Yggdrasil.Activation;
using Machine.Specifications;
using Moq;
using Yggdrasil.Types;
using System;

namespace Yggdrasil.Specs.Activation.for_DefaultStrategy.given
{
	public class a_default_activation_strategy
	{
		protected static DefaultStrategy strategy;
        protected static Mock<IContainer> container_mock;
        protected static Mock<ITypeSystem> type_system_mock;
        protected static Mock<ITypeDefinition> type_definition_mock;

        Establish context = () =>
        {
            container_mock = new Mock<IContainer>();
            type_system_mock = new Mock<ITypeSystem>();
            container_mock.SetupGet(c => c.TypeSystem).Returns(type_system_mock.Object);
            strategy = new DefaultStrategy(container_mock.Object);

            type_definition_mock = new Mock<ITypeDefinition>();
            type_system_mock.Setup(t => t.GetDefinitionFor(Moq.It.IsAny<Type>())).Returns(type_definition_mock.Object);
        };
	}
}
