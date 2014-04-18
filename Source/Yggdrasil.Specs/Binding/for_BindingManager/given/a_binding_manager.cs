using Yggdrasil.Binding;
using Machine.Specifications;
using Moq;
using Yggdrasil.Types;
using System;

namespace Yggdrasil.Specs.Binding.for_BindingManager.given
{
	public class a_binding_manager
	{
		protected static BindingManager binding_manager;
        protected static Mock<ITypeSystem> type_system_mock;
        protected static Mock<ITypeDefinition> type_definition_mock;

        Establish context = () =>
        {
            type_system_mock = new Mock<ITypeSystem>();
            type_definition_mock = new Mock<ITypeDefinition>();
            type_system_mock.Setup(t => t.GetDefinitionFor(Moq.It.IsAny<Type>())).Returns(type_definition_mock.Object);

            binding_manager = new BindingManager(type_system_mock.Object);
        };
	}
}
