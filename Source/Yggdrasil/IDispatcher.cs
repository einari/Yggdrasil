using System;

namespace Yggdrasil
{
	public interface IDispatcher
	{
		bool CheckAccess();
		void BeginInvoke(Action a);
	}
}
