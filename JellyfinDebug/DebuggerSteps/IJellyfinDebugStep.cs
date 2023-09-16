using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace JellyfinDebug.DebuggerSteps
{
	public interface IJellyfinDebugStep
	{
		float Order { get; }
		string Name { get; }
		IAsyncEnumerable<IDebugResult> Execute(IDictionary<string, object> data, CancellationTokenSource abort);
	}
}
