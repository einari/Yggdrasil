namespace Yggdrasil.Specs
{
	public class ClassWithServiceDependency
	{
		public IServiceWithImplementation Service { get; private set; }

		public ClassWithServiceDependency(IServiceWithImplementation service)
		{
			Service = service;
		}
	}
}