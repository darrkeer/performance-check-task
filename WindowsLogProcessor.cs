using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
public class WindowsLogProcessor : LogProcessor
{
    public override void CaptureData()
    {
        throw new NotImplementedException();
    }
}