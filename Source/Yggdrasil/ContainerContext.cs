using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Yggdrasil.Types;

namespace Yggdrasil
{
	public class ContainerContext
	{
		static readonly object LockObject = new object();
		static Container _current;

		public static IContainer Current
		{
			get
			{
				lock( LockObject )
				{
					if( _current == null )
						
						InitializeDefault();

					return _current;
				}
			}
		}

		static void InitializeDefault()
		{
			

#if(NETMF)
            var typeDiscoverer = new TypeDiscoverer();
#else
			var assemblyLocator = new AssemblyLocator();
			var typeDiscoverer = new TypeDiscoverer(assemblyLocator);
#endif
            var typeSystem = new TypeSystem(typeDiscoverer);

            _current = new Container(typeSystem);

			var bindingManager = new BindingManager(typeSystem);
			var strategyActivator = new StrategyActivator(_current);
			var activationManager = new ActivationManager(typeDiscoverer, strategyActivator);
			var bindingDiscoverer = new BindingDiscoverer(_current, activationManager, typeSystem, typeDiscoverer);

			_current.Initialize(bindingManager, bindingDiscoverer, activationManager);
		}
	}
}