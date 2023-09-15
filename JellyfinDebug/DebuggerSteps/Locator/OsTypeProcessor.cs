using System.Diagnostics;

namespace JellyfinDebug.DebuggerSteps.Locator;

public class OsTypeProcessor
{
	public static async Task<OsType> GetType()
	{
		OsType osName = OsType.Unknown;
		if (OperatingSystem.IsIOS())
		{
			osName = OsType.Mac;
		}
		else if (OperatingSystem.IsWindows())
		{
			osName = OsType.Windows;
		}
		else if (OperatingSystem.IsLinux())
		{
			Process p = new Process
			{
				StartInfo = {
					UseShellExecute        = false,
					RedirectStandardOutput = true,
					FileName               = "uname",
					Arguments              = "-s"
				}
			};
			p.Start();
			var uname = (await p.StandardOutput.ReadToEndAsync()).Trim();
			osName = uname switch
			{
				"arch" => OsType.Linux_arch,
				"fedora" => OsType.Linux_Fedora,
				"centOs" => OsType.Linux_CentOS,
				"debian" => OsType.Linux_Debian,
				"ubuntu" => OsType.Linux_Ubuntu,
				"gentoo" => OsType.Linux_Gentoo,
				_ => OsType.Linux
			};
		}

		return osName;
	}
}