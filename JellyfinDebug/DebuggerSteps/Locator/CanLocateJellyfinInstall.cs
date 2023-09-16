using System;
using System.Diagnostics;
using Microsoft.Extensions.Primitives;
using ServiceLocator.Attributes;

namespace JellyfinDebug.DebuggerSteps.Locator;
using Console = JellyfinDebug.ColoredConsole;

[TransientService(typeof(IJellyfinDebugStep))]
public class CanLocateJellyfinInstall : IJellyfinDebugStep
{
	public float Order { get; } = 1;
	public string Name { get; } = "Locate Jellyfin installation.";
	public async IAsyncEnumerable<IDebugResult> Execute(IDictionary<string, object> data, CancellationTokenSource abort)
	{
		yield return new InfoDebugResult("Check Operating System");
		var osType = await OsTypeProcessor.GetType();
		if (osType == OsType.Unknown)
		{
			if (!Console.Ask("Do you run Jellyfin in Docker?"))
			{
				yield return new ErrorDebugInfo("Could not Determine a valid Operating System type.");
				abort.Cancel();
				yield break;
			}

			osType = OsType.Docker;
		}

		yield return new OkDebugResult($"Operating system is supported: {osType}");
		var localJellyfinInstall = new LocalJellyfinInstall(osType);

		var defaultInstallPath = GetDefaultInstallPath(osType);

		if (!Path.Exists(Path.Combine(defaultInstallPath, "jellyfin.exe")))
		{
			yield return new WarnDebugInfo("Jellyfin server installation could not be found.");
			defaultInstallPath = Console.AskStringNonNull("Please input the path to you jellyfin install folder");
			yield return new NoteDebugInfo("Check Path exists.");
			if (!Path.Exists(defaultInstallPath))
			{
				yield return
					new ErrorDebugInfo("The path does not exist. Seems like you did not install Jellyfin.");
				abort.Cancel();
				yield break;
			}
			yield return new OkDebugResult("Manual Installation path Exists.");
			yield return new NoteDebugInfo("Check Path contains Jellyfin.exe.");
			if (!Path.Exists(Path.Combine(defaultInstallPath, "jellyfin.exe")))
			{
				yield return new ErrorDebugInfo("Jellyfin server installation could not be found.")
					.With(new ErrorDebugInfo("The path you entered does not contain the <info>jellyfin.exe</info>."));
				abort.Cancel();
				yield break;
			}
			yield return new OkDebugResult("Jellyfin.exe found.");
		}
		else
		{
			yield return new InfoDebugResult($"Found jellyfin installation at '{defaultInstallPath}'");
		}
		localJellyfinInstall.InstallRoot = defaultInstallPath;

		data["JF"] = localJellyfinInstall;
	}

	public static string? GetDefaultInstallPath(OsType osType)
	{
		switch (osType)
		{
			case OsType.Mac:
				break;
			case OsType.Windows:
				return "C:\\Program Files\\Jellyfin\\Server";
			case OsType.Linux_arch:
				break;
			case OsType.Linux_Fedora:
				break;
			case OsType.Linux_CentOS:
				break;
			case OsType.Linux_Debian:
				break;
			case OsType.Linux_Ubuntu:
				break;
			case OsType.Linux_Gentoo:
				break;
			case OsType.Linux:
				break;
		}

		return null;
	}
}