using JellyfinDebug.DebuggerSteps.Locator;

namespace JellyfinDebug;

public interface IJellyfinInstall
{
    OsType OsType { get; }
    string? InstallRoot { get; }
    string? ConfigPath { get; set; }
    IEnumerable<string> GetInstallFiles();
}