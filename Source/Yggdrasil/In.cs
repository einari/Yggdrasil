using Yggdrasil.Activation;

namespace Yggdrasil
{
	public class In
	{
		public static IScope SingletonScope()
		{
			return new SingletonScope();
		}
	}
}