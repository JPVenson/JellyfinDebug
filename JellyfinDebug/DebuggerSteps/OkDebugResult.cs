using Console = JellyfinDebug.ColoredConsole;
namespace JellyfinDebug.DebuggerSteps;

public class OkDebugResult : IDebugResult
{
	public OkDebugResult(string text)
	{
		Text = text;
	}

	public bool State { get; } = true;
	public string Text { get; set; }

	public ValueTask Render()
	{
		Console.WriteLine($" - [OK] <success>{Text}</success>");
		return ValueTask.CompletedTask;
	}
}

public class InfoDebugResult : IDebugResult
{
	private readonly string _message;

	public InfoDebugResult(string message)
	{
		_message = message;
	}
	public bool State { get; }
	public ValueTask Render()
	{
		Console.WriteLine($" - [INFO] <info>{_message}</info>");
		return ValueTask.CompletedTask;
	}
}

public class ErrorDebugInfo : IDebugResult
{
	private readonly string _text;

	public ErrorDebugInfo(string text)
	{
		_text = text;
	}

	public bool State { get; } = false;
	public ValueTask Render()
	{
		Console.WriteLine($" - [ERR] <error>{_text}</error>");
		return ValueTask.CompletedTask;
	}
}