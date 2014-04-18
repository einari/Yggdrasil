using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using Yggdrasil.Types;
using System;

namespace Yggdrasil.Specs.Binding.for_DefaultBindingConvention.given
{
	public class a_default_binding_convention
	{
		protected static DefaultBindingConvention convention;
        protected static Mock<IContainer> container_mock;
        protected static Mock<ITypeSystem> type_system_mock;
        protected static Mock<ITypeDefinition> type_definition_mock;


        Establish context = () =>
        {
            container_mock = new Mock<IContainer>();
            type_system_mock = new Mock<ITypeSystem>();
            container_mock.SetupGet(c => c.TypeSystem).Returns(type_system_mock.Object);
            convention = new DefaultBindingConvention(container_mock.Object);

            type_definition_mock = new Mock<ITypeDefinition>();
            type_system_mock.Setup(t => t.GetDefinitionFor(Moq.It.IsAny<Type>())).Returns(type_definition_mock.Object);
        };
	}
}
