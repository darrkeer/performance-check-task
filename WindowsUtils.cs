using System.Runtime.Versioning;
using Microsoft.Win32;

[SupportedOSPlatform("windows")]
public class WindwosUtils : Utils
{
    public override string getSteamPath()
    {
        var raw = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string;

        if (raw == null)
        {
            throw new FileNotFoundException("Steam not found");
        }
        return Path.GetFullPath(raw);
    }
}