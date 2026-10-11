using System.Runtime.InteropServices;

public abstract class Utils
{
    public string getGameConfigPath()
    {
        return Path.Combine(
            getSteamPath(), "steamapps", "common", "Black Myth Wukong Benchmark Tool",
            "b1", "Saved", "Config", "Windows", "GameUserSettings.ini"
        );
    }

    public string getOutputFilePath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "output", "log.txt");
    }

    public string getCPUConfigFilePath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "configs", "CPUBenchmark.ini");
    }

    public string getGPUConfigFilePath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "configs", "GPUBenchmark.ini");
    }
    
    public abstract string getSteamPath();

    public static Utils getPlatformUtils()
    {
        if (OperatingSystem.IsWindows())
        {
            return new WindwosUtils();
        }
        else if (OperatingSystem.IsLinux())
        {
            return new LinuxUtils();
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}