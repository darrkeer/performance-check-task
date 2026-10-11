using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
public class WindowsKeyPresser : KeyPresser
{
    public override void Press()
    {
        throw new NotImplementedException();
    }
}