using System;
using Machine.Specifications;

namespace Yggdrasil.Specs.Binding.for_SelfBindingConvention
{
	public class when_getting_target_for_a_class : given.a_self_binding_convention
	{
		static Type result;

		Because of = () => result = convention.GetBindingTarget(typeof (SomeClass));

		It should_return_same_type_as_getting_for = () => result.ShouldEqual(typeof (SomeClass));
	}
}