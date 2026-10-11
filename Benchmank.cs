using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Benchmark
{
    public const int LOG_DURATION_SECONDS = 15;
    public const int LOAD_WAIT_TIME_SECONDS = 30;
    public const string STEAM_APP_ID = "3132990";
    public const string OUTPUT_LOG_PATH = "output/log.txt";

    public string Name { get; }
    private Config config;

    public Benchmark(string name, string configPath)
    {
        Name = name;
        config = new Config(configPath);
    }

    private (HookedLauncher, KeyPresser, LogProcessor) initialize()
    {
        HookedLauncher launcher;
        KeyPresser presser;
        LogProcessor processor;

        if (OperatingSystem.IsWindows())
        {
            launcher = new WindowsHookedLauncher(STEAM_APP_ID);
            presser = new WindowsKeyPresser();
            processor = new WindowsLogProcessor();
        }
        else if (OperatingSystem.IsLinux())
        {
            launcher = new LinuxHookedLauncher(STEAM_APP_ID);
            presser = new LinuxKeyPresser();
            processor = new LinuxLogProcessor();
        }
        else
        {
            throw new NotImplementedException("Your OS is not supported");
        }

        return (launcher, presser, processor);
    }

    public void Start(Utils utils)
    {
        // initialize OS specific stuff
        HookedLauncher launcher;
        KeyPresser presser;
        LogProcessor processor;

        (launcher, presser, processor) = initialize();

        // begin testing
        config.Dump(utils);

        launcher.Launch(utils);

        // prob change to directory regex because it is not os specific
        string wukongProcessName = "GameThread";
        waitUntilStart(wukongProcessName, TimeSpan.FromSeconds(60));

        Thread.Sleep(LOAD_WAIT_TIME_SECONDS * 1000); // waiting for loading

        presser.Press();

        // collect logs
        // wait until log is captured
        Thread.Sleep((LOG_DURATION_SECONDS + 20) * 1000);

        processor.CaptureData();

        GameConfigGrabber grabber = new GameConfigGrabber(config);
        OutputWriter writer = new OutputWriter(OUTPUT_LOG_PATH, Name);
        writer.Write(processor.LastResult, grabber.GrabData());

        killProcessByName(wukongProcessName);
    }

    private void waitUntilStart(string processName, TimeSpan timeout)
    {
        var sw = Stopwatch.StartNew();
        while (sw.Elapsed < timeout)
        {
            var p = Process.GetProcessesByName(processName).FirstOrDefault();
            if (p != null)
            {
                return;
            }
            Thread.Sleep(500);
        }
        throw new Exception($"Process {processName} refused to start!");
    }

    private void killProcessByName(string processName)
    {
        var p = Process.GetProcessesByName(processName).FirstOrDefault();
        if (p == null)
        {
            return;
        }
        p.Kill();
        p.WaitForExit();
    }
}