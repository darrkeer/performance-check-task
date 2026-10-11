using System.Diagnostics;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
public class LinuxHookedLauncher : HookedLauncher
{
    private const int STEAM_SHUTDOWN_WAIT_SECONDS = 10;

    private static readonly List<string> NEEDED_TOKENS = new() {
        "cpu_stats", "gpu_stats", "cpu_temp", "gpu_temp", "ram", "vram"
    }; 

    public LinuxHookedLauncher(string id) : base(id) {}

    public override void Launch(Utils utils)
    {
        // kill steam to change config
        closeSteam();

        CreateMangoHUDConfig();

        // adds a startup argument to a game for MangoHUD
        changeSteamConfig(utils);

        string url = $"steam://rungameid/{gameId}";
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

        Console.WriteLine("Game Launched");
    }

    private void closeSteam()
    {
        if (Process.GetProcessesByName("steam").Length == 0)
        {
            return;
        }

        Process.Start(new ProcessStartInfo("steam", "-shutdown") { UseShellExecute = false })?.WaitForExit();

        for (int i = 0; i < STEAM_SHUTDOWN_WAIT_SECONDS; i++)
        {
            Thread.Sleep(1000);
            if (Process.GetProcessesByName("steam").Length == 0)
            {
                return;
            }
        }
        throw new Exception("Could not wait for steam for a shutdown");
    } 

    private void changeSteamConfig(Utils utils)
    {
        string steamPath = utils.getSteamPath();

        string? userid = Directory.EnumerateDirectories(Path.Combine(steamPath, "userdata"))
            .Select(Path.GetFileName)
            .FirstOrDefault();
        if (userid == null)
        {
            throw new Exception("Log into a steam before launching");
        }

        string configFilePath = Path.Combine(steamPath, "userdata", userid, "config", "localconfig.vdf");
        
        // need to size up to check through all of the steam config file
        var settings = new VdfSerializerSettings
        {
            MaximumTokenSize = 1 << 20,
            UsesEscapeSequences = true
        };

        string currentConfigFilePath = getMangoHUDConfigPath();
        string launchArgs = $"MANGOHUD_CONFIGFILE={currentConfigFilePath} mangohud %command%";

        Console.WriteLine($"Updated app argumetns: {launchArgs}");

        // UserLocalConfigStore/Software/Valve/Steam/apps/<AppID>/LaunchOptions
        VProperty cfg = VdfConvert.Deserialize(File.ReadAllText(configFilePath), settings)!;
        cfg.Value["Software"]?["Valve"]?["Steam"]?["apps"]?[gameId]?["LaunchOptions"] = new VValue(launchArgs);

        File.Copy(configFilePath, configFilePath + ".bak", overwrite: true);
        File.WriteAllText(configFilePath, VdfConvert.Serialize(cfg, settings));

        Console.WriteLine($"Adjusted {configFilePath} to run via MangoHUD");
    }

    public static string getMangoHUDConfigPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "configs", "mangohud.conf");
    }

    public static string getMangoHUDOutputDirPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "output", "mangohud");
    }

    private void CreateMangoHUDConfig()
    {
        string confPath = getMangoHUDConfigPath();
        Directory.CreateDirectory(Path.GetDirectoryName(confPath)!);
        
        string mangohudOutDirPath = getMangoHUDOutputDirPath();
        Directory.CreateDirectory(mangohudOutDirPath);

        using var writer = new StreamWriter(confPath);
        
        writer.WriteLine($"output_folder={mangohudOutDirPath}");
        writer.WriteLine($"log_interval=100");
        // wait 10 seconds for macros and loading into benchmark
        writer.WriteLine($"autostart_log={Benchmark.LOAD_WAIT_TIME_SECONDS + 20}");
        writer.WriteLine($"log_duration={Benchmark.LOG_DURATION_SECONDS}");
        writer.WriteLine();

        foreach (string token in NEEDED_TOKENS)
        {
            writer.WriteLine(token);
        }
    }
}