using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
public class LinuxKeyPresser : KeyPresser
{
    public override void Press()
    {
        var daemon = StartProcess
        (
            "ydotoold", $"--socket-path=/run/user/{Posix.GetUid()}/.ydotool_socket",
             $"--socket-own={Posix.GetUid()}:{Posix.GetGid()}"
        );
        Thread.Sleep(5000); // waiting for daemon start

        Run("ydotool", "click", "0xC0");
        Thread.Sleep(1500);

        Run("ydotool", "mousemove", "--absolute", "-x", "100", "-y", "235");
        Thread.Sleep(1500);

        Run("ydotool", "click", "0xC0");
        Thread.Sleep(1500);

        Run("ydotool", "mousemove", "--absolute", "-x", "400", "-y", "310");
        Thread.Sleep(1500);

        Run("ydotool", "click", "0xC0");
        Thread.Sleep(1500);

        daemon.Kill();
        daemon.WaitForExit();
    }

    static class Posix
    {
        [DllImport("libc", EntryPoint = "getuid")] public static extern uint GetUid();
        [DllImport("libc", EntryPoint = "getgid")] public static extern uint GetGid();
    }
}