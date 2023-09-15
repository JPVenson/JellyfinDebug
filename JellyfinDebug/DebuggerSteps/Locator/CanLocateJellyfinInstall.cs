using System;
using System.Diagnostics;

namespace JellyfinDebug.DebuggerSteps.Locator;
using Console = JellyfinDebug.ColoredConsole;

public class CanLocateJellyfinInstall : IJellyfinDebugStep
{
	public string Name { get; } = "Locate Jellyfin installation.";
	public async IAsyncEnumerable<IDebugResult> Execute(IDictionary<string, object> data)
	{
		yield return new InfoDebugResult("Check Operating System");
		var osType = await OsTypeProcessor.GetType();
		if (osType == OsType.Unknown)
		{
			if (!Console.Ask("Do you run Jellyfin in Docker?"))
			{
				yield return new ErrorDebugInfo("Could not Determine a valid Operating System type.");
				yield break;
			}

			osType = OsType.Docker;
		}

		yield return new OkDebugResult($"Operating system is supported: {osType}");

		var jellyfinProcesses = FindJellyfinProcess();
		if (jellyfinProcesses.Length == 0)
		{
			yield return new ErrorDebugInfo("Jellyfin server process does not run.");
			data["INSTALLPATH"] = GetDefaultInstallPath(osType);
			if (data["INSTALLPATH"] == null)
			{

			}
		}
		else if (jellyfinProcesses.Length > 1)
		{
			yield return new ErrorDebugInfo("Found Multiple jellyfin server processes. This is potentially dangerous if multiple Jellyfin server processes try to access the same database.");
			foreach (var jellyfinProcess in jellyfinProcesses)
			{
				yield return new InfoDebugResult(
					$"{jellyfinProcess.ProcessName} - '{jellyfinProcess.StartInfo.FileName}': {jellyfinProcess.StartInfo.Arguments}");
			}
		}
		else
		{
			var process = jellyfinProcesses[0];
			yield return new OkDebugResult("Found exactly one jellyfin server process.");
			data["INSTALLPATH"] = Path.GetDirectoryName(process.StartInfo.FileName);
		}
	}

	private Process[] FindJellyfinProcess()
	{
		return Process.GetProcessesByName("Jellyfin.exe");
	}

	private string GetDefaultInstallPath(OsType osType)
	{
		switch (osType)
		{
			case OsType.Mac:
				break;
			case OsType.Windows:
				break;
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
	}
}