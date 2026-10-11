public class Config
{
    public string path;
    
    public Config(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Config file not found: {filePath}");
        }
        path = Path.GetFullPath(filePath);
    }

    public void Dump(Utils utils)
    {
        string destPath = utils.getGameConfigPath();
        File.Copy(path, destPath, overwrite: true);

        Console.WriteLine($"Config '{path}' loaded!");
    }
}