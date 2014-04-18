using System;
using System.Collections;
#if(!NETMF)
using System.Collections.Generic;
#endif


namespace Yggdrasil.Activation
{
	public class ActivationManager : IActivationManager
	{
		ITypeDiscoverer _discoverer;
		IStrategyActivator _strategyActivator;
#if(NETMF)
		ArrayList _strategies = new ArrayList();
#else
        List<IStrategy> _strategies = new List<IStrategy>();
#endif

		public ActivationManager(ITypeDiscoverer discoverer, IStrategyActivator strategyActivator)
		{
			_discoverer = discoverer;
			_strategyActivator = strategyActivator;
		}

		void DiscoverStrategies()
		{
			var strategyTypes = _discoverer.FindMultiple(typeof(IStrategy));
            foreach (var strategyType in strategyTypes)
            {
                var instance = _strategyActivator.GetInstance(strategyType);
                if (instance != null)
                {
                    _strategies.Add(instance);
                }
            }
		}

		public IStrategy GetStrategyFor(Type type)
		{
			if( _strategies.Count == 0 )
				DiscoverStrategies();


            foreach (IStrategy strategy in _strategies)
                if (strategy.CanActivate(type)) return strategy;

            return null;
		}
	}
}