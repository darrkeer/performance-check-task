using System.Runtime.InteropServices;

public abstract class SpecsGrabber
{
    protected Dictionary<string, string> Specs { get; private set; }

    public SpecsGrabber()
    {
        Specs = new Dictionary<string, string>();
    }

    public abstract void Grab();

    public void Write(Utils utils)
    {
        Grab();

        using var writer = new StreamWriter(utils.getOutputFilePath(), append: true);

        writer.WriteLine("Specs info:");
        foreach (var (key, value) in Specs)
        {
            writer.WriteLine($"{key}:\t{value}");
        }
    }

    public static SpecsGrabber getPlatformSpecsGrabber()
    {
        if (OperatingSystem.IsWindows())
        {
            return new WindowsSpecsGrabber();
        }
        else if (OperatingSystem.IsLinux())
        {
            return new LinuxSpecsGrabber();
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}