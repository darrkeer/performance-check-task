public class OutputWriter
{
    const string delimiter = "=================================================";
    private string outFilePath;
    private string testName;

    public OutputWriter(string outFilePath, string testName)
    {
        this.outFilePath = outFilePath;
        this.testName = testName;
    }

    public void Write(Dictionary<string, float> testStats, Dictionary<string, string> presets)
    {
        Console.WriteLine("Writing logs");

        string fullPath = Path.GetFullPath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        string outputLogPath = Path.GetFullPath(outFilePath);
        
        using var writer = new StreamWriter(outputLogPath, append: true);

        writer.WriteLine();
        writer.WriteLine($"{testName} test:");
        writer.WriteLine(delimiter);

        writer.WriteLine("Game Presets:");
        
        foreach (var (key, value) in presets)
        {
            writer.WriteLine($"{key}:\t{value}");
        }

        writer.WriteLine();
        writer.WriteLine("Test results:");
        writer.WriteLine(delimiter);
        
        foreach (var (key, value) in testStats)
        {
            writer.WriteLine($"{key}:\t{value}");
        }

        Console.WriteLine($"Log written to file {fullPath}");
    }
}