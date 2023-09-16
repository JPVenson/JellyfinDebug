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

		CancellationTokenSource cts = new CancellationTokenSource();
		foreach (var jellyfinDebugStep in _debugSteps)
		{
			Console.WriteLine(jellyfinDebugStep.Name);
			var result = jellyfinDebugStep.Execute(checkData, cts);

			IDebugResult previousNotification = null;
			await foreach (var notification in result)
			{
				if (notification is UpdateState updateNotification && previousNotification is not null)
				{
					updateNotification.Update(previousNotification);
					await previousNotification.Render();
					continue;
				}

				await notification.Render();
				previousNotification = notification;
			}

			if (cts.IsCancellationRequested)
			{
				Console.WriteLine("<error>Abort</error> Cannot continue as an critical issue was detected.");
				break;
			}
		}

		Console.WriteLine("Press any key to close.");
		System.Console.ReadKey(true);
	}
}