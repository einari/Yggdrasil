using System.Collections.Generic;
using System.Reflection;

namespace Yggdrasil
{
	public interface IAssemblyLocator
	{
		IEnumerable<Assembly> GetAll();
	}
}