using System;
using System.Threading.Tasks;

namespace Tnb
{
	public interface IAppHandler
	{
		Task<bool> LaunchApp(string uri);
	}
}
