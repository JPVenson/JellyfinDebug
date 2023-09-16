using JellyfinDebug.DebuggerSteps.Locator;

namespace JellyfinDebug;

public class LocalJellyfinInstall : IJellyfinInstall
{
    public OsType OsType { get; }

    public LocalJellyfinInstall(OsType osType)
    {
        OsType = osType;
    }

    public string InstallRoot { get; set; }
    public string? ConfigPath { get; set; }

    public IEnumerable<string> GetInstallFiles()
    {
        foreach (var enumerateFile in Directory.EnumerateFiles(InstallRoot))
        {
            yield return enumerateFile;
        }
    }
}