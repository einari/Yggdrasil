using Yggdrasil.Activation;
using Machine.Specifications;

namespace Yggdrasil.Specs.Activation.for_SingletonScope
{
	public class when_getting_twice_after_setting_once
	{
		static SingletonScope scope;
		static object instance_to_set;
		static object first_get;
		static object second_get;


		Establish context = () =>
		                    	{
									instance_to_set = new object();
		                    		scope = new SingletonScope();
		                    	};

		Because of = () =>
		             	{
							scope.SetInstance(typeof(object), instance_to_set);
		             		first_get = scope.GetInstance(typeof (object));
							second_get = scope.GetInstance(typeof(object));
		             	};

		It should_return_same_instance_both_times = () => first_get.ShouldEqual(second_get);
	}
}
