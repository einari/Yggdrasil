using System;

namespace Yggdrasil.Execution
{
	public interface IDispatcher
	{
		bool CheckAccess();
		void BeginInvoke(Action a);
	}
}
