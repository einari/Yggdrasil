using System;
using System.Collections;
#if(!NETMF)
using System.Collections.Generic;
#endif
using Yggdrasil.Types;

namespace Yggdrasil.Binding
{
	public class BindingManager : IBindingManager
	{
        ITypeSystem _typeSystem;
#if(NETMF)
        Hashtable _bindings = new Hashtable();
#else
        Dictionary<Type, IBinding> _bindings = new Dictionary<Type, IBinding>();
#endif

        public BindingManager(ITypeSystem typeSystem)
        {
            _typeSystem = typeSystem;
        }


		public bool HasBinding(Type type)
		{
#if(NETMF)
            return _bindings.Contains(type);
#else
            return _bindings.ContainsKey(type);
#endif
		}

		public IBinding GetBinding(Type type)
		{
            return _bindings[type] as IBinding;
		}

		public void Register(IBinding binding)
		{
			if( binding.Scope == null && 
                binding.Target != null )
			{
                if (_typeSystem.GetDefinitionFor(binding.Target).HasSingletonAttribute) 
                    binding.Scope = In.SingletonScope();
			}

			_bindings[binding.Service] = binding;
		}
	}
}