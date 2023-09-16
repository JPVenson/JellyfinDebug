using System.Data;
using System.Linq;
using Console = JellyfinDebug.ColoredConsole;

namespace JellyfinDebug.DebuggerSteps;

public interface IDebugResult
{
	ValueTask Render();
}

public enum NotificationIcon
{
	OK,
	WARN,
	ERR,
	INF,
	NONE
}

public abstract record DebugResultBase(string Text, string? TextType) : IDebugResult
{
	public NotificationIcon Icon { get; set; }
	public IList<IDebugResult> SubText { get; } = new List<IDebugResult>();

	public bool IsStateful { get; set; }
	public string Text { get; set; } = Text;
	public string? TextType { get; set; } = TextType;

	private ((int row, int column) from, (int row, int column) to)? _stateWindow;

	public DebugResultBase With(IDebugResult result)
	{
		SubText.Add(result);
		return this;
	}

	public DebugResultBase Updatable()
	{
		IsStateful = true;
		return this;
	}

	public async ValueTask Render()
	{
		var iconType = Icon switch
		{
			NotificationIcon.ERR => "ERR",
			NotificationIcon.INF => "INF",
			NotificationIcon.OK => "OK",
			NotificationIcon.WARN => "WARN",
			_ => "NOP"
		};
		var textType = Icon switch
		{
			NotificationIcon.ERR => "error",
			NotificationIcon.INF => "info",
			NotificationIcon.NONE => null,
			NotificationIcon.OK => "success",
			NotificationIcon.WARN => "warning"
		};


		(int row, int column) state = default;
		(int row, int column) restoreState = default;
		if (IsStateful)
		{
			state = (System.Console.CursorTop, System.Console.CursorLeft);
			if (!_stateWindow.Equals(default))
			{
				restoreState = (System.Console.CursorTop, System.Console.CursorLeft);
				System.Console.CursorTop = _stateWindow.Value.Item1.row;
				System.Console.CursorLeft = _stateWindow.Value.Item1.column;

				var toClear = (_stateWindow.Value.to.row - _stateWindow.Value.from.row) * System.Console.WindowWidth;
				Console.Write(string.Join("", Enumerable.Repeat(" ", toClear)));
				System.Console.CursorTop = _stateWindow.Value.Item1.row;
				System.Console.CursorLeft = _stateWindow.Value.Item1.column;
			}
		}

		string text;
		if (textType is null)
		{
			text = Text;
		}
		else
		{
			text = $"<{textType}>{Text}</{textType}>";
		}
		Console.WriteLine($" - [{iconType}] {text}");
		foreach (var debugResult in SubText)
		{
			Console.Write("\t");
			await debugResult.Render();
		}

		if (IsStateful)
		{
			_stateWindow = (state, ((System.Console.CursorTop, System.Console.CursorLeft)));
		}

		if (restoreState != default)
		{
			System.Console.CursorTop = restoreState.row;
			System.Console.CursorLeft = restoreState.column;
		}
	}
}