using System.Collections.Generic;
using System.Reflection;

namespace Yggdrasil.Execution
{
	public interface IAssemblyLocator
	{
		IEnumerable<Assembly> GetAll();
	}
}