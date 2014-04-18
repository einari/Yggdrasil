using System;
using System.Collections;
#if(!NETMF)
using System.Collections.Generic;
#endif

namespace Yggdrasil.Activation
{
	public class SingletonScope : IScope
	{
#if(NETMF)
        Hashtable _instances = new Hashtable();
#else
        Dictionary<Type, object> _instances = new Dictionary<Type, object>();
#endif

		public bool IsInScope(Type type)
		{
#if(NETMF)
            return _instances.Contains(type);
#else
            return _instances.ContainsKey(type);
#endif
		}

		public object GetInstance(Type type)
		{
			return _instances[type];
		}

		public void SetInstance(Type type, object instance)
		{
			_instances[type] = instance;
		}
	}
}