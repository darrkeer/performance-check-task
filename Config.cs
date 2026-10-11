public class Config
{
    public string FilePath { get; private set; }

    public Config(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Config file not found: {filePath}");
        }
        FilePath = System.IO.Path.GetFullPath(filePath);
    }

    public void Dump(Utils utils)
    {
        string destPath = utils.getGameConfigPath();
        File.Copy(FilePath, destPath, overwrite: true);

        Console.WriteLine($"Config '{FilePath}' loaded!");
    }
}