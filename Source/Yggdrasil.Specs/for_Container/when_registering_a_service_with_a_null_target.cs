using System;
using Yggdrasil.Activation;
using Machine.Specifications;

namespace Yggdrasil.Specs.for_Container
{
    public class when_registering_a_service_with_a_null_target : given.a_container
    {
        static Exception exception;

        Because of = () => exception = Catch.Exception(() => container.Register(typeof(IServiceWithoutImplementation), (Type)null));

        It should_throw_missing_target_type_exception = () => exception.ShouldBeOfType<MissingTargetTypeException>();
        It should_hold_the_correct_service_type = () => ((MissingTargetTypeException)exception).Service.ShouldEqual(typeof(IServiceWithoutImplementation));
    }
}
