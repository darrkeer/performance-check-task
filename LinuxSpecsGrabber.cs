using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
public class LinuxSpecsGrabber : SpecsGrabber
{
    public override void Grab()
    {
        string logDir = LinuxHookedLauncher.getMangoHUDOutputDirPath();
        string? file = new DirectoryInfo(logDir)
            .EnumerateFiles()
            .Where(f => !f.Name.Contains("summary", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(f => f.LastWriteTimeUtc)
            .FirstOrDefault()?.FullName;

        if (file == null)
        {
            throw new Exception("MangoHUD log result not found");
        }

        var lines = File.ReadAllLines(file);
        var specNames = lines[0].Split(',');
        var values = lines[1].Split(',');
        for (int i = 0; i < specNames.Length; ++i)
        {
            Specs[specNames[i]] = values[i];
        }
    }
}