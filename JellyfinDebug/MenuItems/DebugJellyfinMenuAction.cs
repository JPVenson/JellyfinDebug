using JellyfinDebug.DebuggerSteps;
using ServiceLocator.Attributes;
using Console = JellyfinDebug.ColoredConsole;

namespace JellyfinDebug.MenuItems;

[TransientService(typeof(IDebugAction))]
public class DebugJellyfinMenuAction : IDebugAction
{
	private readonly IEnumerable<IJellyfinDebugStep> _debugSteps;

	public DebugJellyfinMenuAction(IEnumerable<IJellyfinDebugStep> debugSteps)
	{
		_debugSteps = debugSteps;
	}

	public string Name { get; } = "Debug Jellyfin.";
	public async ValueTask Execute()
	{
		Console.WriteLine("Debug Jellyfin Installation on this Device...");
		var checkData = new Dictionary<string, object>();

		foreach (var jellyfinDebugStep in _debugSteps)
		{
			Console.Write(jellyfinDebugStep.Name);
			var result = jellyfinDebugStep.Execute(checkData);
			await foreach (var notification in result)
			{
				await notification.Render();
			}
		}
	}
}