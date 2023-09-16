using JellyfinDebug.DebuggerSteps.Locator;
using ServiceLocator.Attributes;
using System.IO;
using Console = JellyfinDebug.ColoredConsole;

namespace JellyfinDebug.DebuggerSteps.ConfigTester
{
    [TransientService(typeof(IJellyfinDebugStep))]
	internal class JellyfinConfigTester : IJellyfinDebugStep
	{
		public float Order { get; } = 3;
		public string Name { get; } = "Evaluate jellyfin Configuration";
		public async IAsyncEnumerable<IDebugResult> Execute(IDictionary<string, object> data, CancellationTokenSource abort)
		{
			var jf = data["JF"] as IJellyfinInstall;
			var configPath = GetDefaultInstallDataFolder(jf.OsType, jf);

			var pathVariantses = configPath as string[] ?? configPath.ToArray();
			if (!pathVariantses.Any())
			{
				yield return new WarnDebugInfo("Could not determine Jellyfin Server Configuration folder.");
				var path = Console.AskStringNonNull("Jellyfin Server Configuration Folder: ");
				pathVariantses = new[] { path };
			}

			var configPathCandidates = new List<string>();
			foreach (var pathVariants in pathVariantses)
			{
				var path = pathVariants;
				yield return new NoteDebugInfo($"Check for Jellyfin config in '{path}'").Updatable();
				
				if (!Directory.Exists(path))
				{
					yield return new UpdateState(NotificationIcon.WARN, $"Could not find Jellyfin Config at: {path}");
					continue;
				}

				configPathCandidates.Add(path);
			}

			if (!configPathCandidates.Any())
			{
				yield return new ErrorDebugInfo("Jellyfin Config directory is empty or could not be found.")
					.With(new NoteDebugInfo("This means the Jellyfin server never started or is misconfigured"));
				abort.Cancel();
				yield break;
			}

			var expectedFolders = new[]
			{
				"cache",
				"config",
				"data",
				"log",
				"metadata",
				"plugins",
				"root"
			};
			foreach (var configPathCandidate in configPathCandidates)
			{
				yield return new InfoDebugResult($"Check contents of '{configPathCandidate}'");
				var allFound = true;
				foreach (var expectedFolder in expectedFolders)
				{
					yield return new NoteDebugInfo($"Check for folder '{expectedFolder}'").Updatable();
					var expectedPath = Path.Combine(configPathCandidate, expectedFolder);
					if (!Directory.Exists(expectedPath))
					{
						yield return new UpdateState(NotificationIcon.WARN, $"Did not find '{expectedFolder}' under: '{expectedPath}'");
						allFound = false;
					}
					else
					{
						yield return new UpdateState(NotificationIcon.OK, $"Found '{expectedFolder}' in '{expectedPath}'");
					}
				}

				if (allFound)
				{
					yield return new OkDebugResult($"Most likely config path is: {configPathCandidate}.");
					jf.ConfigPath = configPathCandidate;
					break;
				}
			}

			if (jf.ConfigPath is null)
			{
				yield return new ErrorDebugInfo("Could not find any folder that may contain your jellyfin server Configuration.")
					.With(new NoteDebugInfo("This can be if you never started the jellyfin server."));
				abort.Cancel();
			}
		}

		public static IEnumerable<string> GetDefaultInstallDataFolder(OsType osType, IJellyfinInstall jellyfinInstall)
		{
			if (jellyfinInstall == null) throw new ArgumentNullException(nameof(jellyfinInstall));
			switch (osType)
			{
				case OsType.Unknown:
					break;
				case OsType.Mac:
					break;
				case OsType.Windows:
					foreach (var specialFolder in Enum.GetValues<Environment.SpecialFolder>())
					{
						yield return Path.Combine(Environment.GetFolderPath(specialFolder), "Jellyfin", "Server");
					}
					yield return Path.Combine(jellyfinInstall.InstallRoot, "Config");
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
				default:
					throw new ArgumentOutOfRangeException(nameof(osType), osType, null);
			}
		}
	}
}
