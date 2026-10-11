using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
public class LinuxLogProcessor : LogProcessor
{
    private string getLastLogPath()
    {
        string logDir = LinuxHookedLauncher.getMangoHUDOutputDirPath();
        string? file = new DirectoryInfo(logDir)
            .EnumerateFiles("*summary*")
            .OrderByDescending(f => f.LastWriteTimeUtc)
            .FirstOrDefault()?.FullName;
        if (file == null)
        {
            throw new Exception($"MangoHUD log file not found in {logDir}");
        }
        return file;
    }

    public override void CaptureData()
    {
        LastResult.Clear();

        var stream = new StreamReader(getLastLogPath());

        var tokens = stream.ReadLine()!.Split(',');
        var values = stream.ReadLine()!.Split(',');

        for (int i = 0; i < tokens.Length; i++)
        {
            LastResult[tokens[i]] = float.Parse(values[i]);
        }
    }
}