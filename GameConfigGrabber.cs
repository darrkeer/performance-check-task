using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

public class GameConfigGrabber
{
    private string FilePath { get; }

    public GameConfigGrabber(Config config)
    {
        FilePath = config.FilePath;
    }

    public Dictionary<string, string> GrabData()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
        var lines = File.ReadAllLines(path);

        Dictionary<string, string> result = new Dictionary<string, string>();
        var options = lines[7].Split('=')[1][1..^1];

        foreach (Match m in Regex.Matches(options, "\\(\"([^\"]*)\"\\s*,\\s*\"([^\"]*)\"\\)"))
            result[m.Groups[1].Value] = m.Groups[2].Value;

        return result;
    }
}