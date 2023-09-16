using ServiceLocator.Attributes;
using System.Diagnostics;
using Console = JellyfinDebug.ColoredConsole;

namespace JellyfinDebug.DebuggerSteps.JellyfinRun
{
	[TransientService(typeof(IJellyfinDebugStep))]
	internal class JellyfinRunTester : IJellyfinDebugStep
	{
		public float Order { get; } = 2;
		public string Name { get; } = "Check Jellyfin Server runs.";
		public async IAsyncEnumerable<IDebugResult> Execute(IDictionary<string, object> data, CancellationTokenSource abort)
		{
			var jellyfinProcesses = FindJellyfinProcess();
			if (jellyfinProcesses.Length == 0)
			{
				yield return new ErrorDebugInfo("Jellyfin server process does not run.");
			}
			else if (jellyfinProcesses.Length > 1)
			{
				yield return new ErrorDebugInfo("Found Multiple jellyfin server processes. ")
					.With(new NoteDebugInfo("This is potentially dangerous if multiple Jellyfin server processes try to access the same database."))
					.With(new NoteDebugInfo("Please only run One jellyfin instance at a time."));
				foreach (var jellyfinProcess in jellyfinProcesses)
				{
					yield return new InfoDebugResult(
						$"{jellyfinProcess.ProcessName} - '{jellyfinProcess.StartInfo.FileName}': {jellyfinProcess.StartInfo.Arguments}");
				}
				abort.Cancel();
			}
			else
			{
				var process = jellyfinProcesses[0];
				yield return new OkDebugResult("Found exactly one jellyfin server process.");
			}
		}

		private Process[] FindJellyfinProcess()
		{
			return Process.GetProcessesByName("Jellyfin");
		}
	}
}
