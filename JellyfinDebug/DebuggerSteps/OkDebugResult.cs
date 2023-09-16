using static System.Net.Mime.MediaTypeNames;
using Console = JellyfinDebug.ColoredConsole;
namespace JellyfinDebug.DebuggerSteps;

public record OkDebugResult : DebugResultBase
{
	public OkDebugResult(string text) : base(text, "success")
	{
		Icon = NotificationIcon.OK;
	}
}
public record UpdateState : DebugResultBase
{
	public UpdateState(NotificationIcon notificationIcon, string? text = null) : base(text, null)
	{
		Icon = notificationIcon;
	}

	public void Update(IDebugResult previousNotification)
	{
		if (previousNotification is DebugResultBase updatable)
		{
			updatable.Icon = Icon;
			updatable.Text = Text ?? updatable.Text;
		}
	}
}

public record InfoDebugResult : DebugResultBase
{
	public InfoDebugResult(string text) : base(text, "info")
	{
		Icon = NotificationIcon.INF;
	}
}

public record ErrorDebugInfo : DebugResultBase
{
	public ErrorDebugInfo(string text) : base(text, $"error")
	{
		Icon = NotificationIcon.ERR;
	}
}

public record WarnDebugInfo : DebugResultBase
{
	public WarnDebugInfo(string text) : base(text, $"warn")
	{
		Icon = NotificationIcon.WARN;
	}
}

public record NoteDebugInfo : DebugResultBase
{
	public NoteDebugInfo(string text) : base(text, null)
	{
		Icon = NotificationIcon.NONE;
	}
}