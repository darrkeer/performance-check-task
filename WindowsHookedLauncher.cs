using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
public class WindowsHookedLauncher : HookedLauncher
{
    public WindowsHookedLauncher(string id) : base(id) {}

    public override void Launch(Utils utils)
    {
        throw new NotImplementedException();
    }
}