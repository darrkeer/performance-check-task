using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
public class LinuxUtils : Utils
{
    public override string getSteamPath()
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(home, ".local", "share", "Steam");
    }
}