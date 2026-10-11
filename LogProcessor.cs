public abstract class LogProcessor
{
    public Dictionary<string, float> LastResult { get; }

    public LogProcessor()
    {
        LastResult = new Dictionary<string, float>();
    }

    public abstract void CaptureData();
}
