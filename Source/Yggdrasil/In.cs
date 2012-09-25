using Yggdrasil.Execution.Activation;

namespace Yggdrasil.Execution
{
	public class In
	{
		public static IScope SingletonScope()
		{
			return new SingletonScope();
		}
	}
}