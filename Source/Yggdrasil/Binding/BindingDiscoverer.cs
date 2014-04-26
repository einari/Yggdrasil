using System;
using System.Collections;
#if(!NETMF)
using System.Collections.Generic;
#endif
using System.Reflection;
using Yggdrasil.Activation;
using Yggdrasil.Types;

namespace Yggdrasil.Binding
{
	public class BindingDiscoverer : IBindingDiscoverer
	{
#if(NETMF)
        ArrayList _conventions = new ArrayList();
#else
        List<IBindingConvention> _conventions = new List<IBindingConvention>();
#endif
		IActivationManager _activationManager;

        public BindingDiscoverer(IContainer container, IActivationManager activationManager, ITypeSystem typeSystem, ITypeDiscoverer typeDiscoverer)
		{
			var conventionTypes = typeDiscoverer.FindMultiple(typeof(IBindingConvention));
            foreach (var conventionType in conventionTypes)
            {
                var instance = typeSystem.GetDefinitionFor(conventionType).CreateInstance(container);
                _conventions.Add((IBindingConvention)instance);
            }

			_activationManager = activationManager;
		}

		public void AddConvention(IBindingConvention convention)
		{
			_conventions.Add(convention);
		}

		public IBinding Discover(Type type)
		{
            foreach (IBindingConvention convention in _conventions)
            {
                if (convention.CanBeBound(type))
                {
                    var target = convention.GetBindingTarget(type);
                    var activationStrategy = _activationManager.GetStrategyFor(target);
                    var binding = new StandardBinding(type, target, activationStrategy);
                    return binding;
                    
                }
            }
			return null;
		}
	}
}