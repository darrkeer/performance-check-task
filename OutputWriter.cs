public class OutputWriter
{
    const string delimiter = "=================================================";
    private string OutFilePath { get; }
    private string TestName { get; }

    public OutputWriter(string outFilePath, string testName)
    {
        OutFilePath = outFilePath;
        TestName = testName;
    }

    public void Write(Dictionary<string, float> testStats, Dictionary<string, string> presets)
    {
        Console.WriteLine("Writing logs");

        string fullPath = Path.GetFullPath(OutFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        string outputLogPath = Path.GetFullPath(OutFilePath);

        using var writer = new StreamWriter(outputLogPath, append: true);

        writer.WriteLine();
        writer.WriteLine($"{TestName} test:");
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