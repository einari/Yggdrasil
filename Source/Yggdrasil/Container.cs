using System;
using Yggdrasil.Activation;
using Yggdrasil.Binding;
using Yggdrasil.Types;

namespace Yggdrasil
{
	public class Container : IContainer
	{
		IBindingManager _bindingManager;
		IBindingDiscoverer _bindingDiscoverer;
		IActivationManager _activationManager;

		public Container(IBindingManager bindingManager, IBindingDiscoverer bindingDiscoverer, IActivationManager activationManager, ITypeSystem typeSystem)
		{
			_bindingManager = bindingManager;
			_bindingDiscoverer = bindingDiscoverer;
			_activationManager = activationManager;
            TypeSystem = typeSystem;
		}

		internal Container(ITypeSystem typeSystem) 
        {
            TypeSystem = typeSystem;
        }

		internal void Initialize(IBindingManager bindingManager, IBindingDiscoverer bindingDiscoverer, IActivationManager activationManager)
		{
			_bindingManager = bindingManager;
			_bindingDiscoverer = bindingDiscoverer;
			_activationManager = activationManager;
			Register(typeof(IContainer),this);
		}

        public ITypeSystem TypeSystem { get; private set; }

#if(!NETMF)
		public T Get<T>()
		{
			var service = typeof(T);
			var instance = Get(service);
			return (T) instance;
		}

		public void Register<TS, TT>(IScope scope = null) where TT : TS
		{
			Register(typeof (TS), typeof (TT));
		}

		public void Register<TS>(TS instance, IScope scope = null)
		{
			Register(typeof(TS), instance);
		}
#endif

        public object Get(Type service)
		{
			IBinding binding = null;
			if( _bindingManager.HasBinding(service) )
				binding = _bindingManager.GetBinding(service);

			if (binding == null)
			{
				binding = _bindingDiscoverer.Discover(service);

				if (binding != null)
					_bindingManager.Register(binding);
			}

			return binding != null ? Activate(binding) : null;
		}


		public void Register(Type service, Type target, IScope scope = null)
		{
            ThrowIfTargetIsMissing(service, target);

			var strategy = _activationManager.GetStrategyFor(target);
			var binding = new StandardBinding(service, target, strategy) {Scope = scope};
			_bindingManager.Register(binding);
		}

		public void Register(Type service, object instance, IScope scope = null)
		{
			var strategy = new ConstantStrategy(instance);
			var binding = new StandardBinding(service, null, strategy) {Scope = scope};
			_bindingManager.Register(binding);
		}

		static object Activate(IBinding binding)
		{
			if( binding.Scope != null && binding.Scope.IsInScope(binding.Target) )
				return binding.Scope.GetInstance(binding.Target);

			var instance = binding.Strategy.GetInstance(binding.Target);
			if (binding.Scope != null)
				binding.Scope.SetInstance(binding.Target, instance);

			return instance;
		}


        void ThrowIfTargetIsMissing(Type service, Type target)
        {
            if (target == null)
                throw new MissingTargetTypeException(service);
        }
	}
}