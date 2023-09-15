using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyfinDebug.DebuggerSteps
{
	public interface IJellyfinDebugStep
	{
		string Name { get; }
		IAsyncEnumerable<IDebugResult> Execute(IDictionary<string, object> data);
	}
}
