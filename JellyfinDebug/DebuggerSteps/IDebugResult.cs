namespace JellyfinDebug.DebuggerSteps;

public interface IDebugResult
{
	bool State { get; }
	ValueTask Render();
}