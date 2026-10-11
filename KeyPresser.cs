using System.Diagnostics;

public abstract class KeyPresser
{

    public abstract void Press();

    protected static Process StartProcess(string file, params string[] args)
    {
        var psi = new ProcessStartInfo(file)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        foreach (var a in args) psi.ArgumentList.Add(a);

        string cmd = file + " ";
        foreach (string arg in args)
        {
            cmd += arg + " ";
        }
        Console.WriteLine($"Ran: {cmd}");

        var p = Process.Start(psi)!;

        // handle the error output
        p.ErrorDataReceived += (s, e) =>
        {
            if (e.Data != null) Console.WriteLine(e.Data);
        };
        p.OutputDataReceived += (s, e) =>
        {
            if (e.Data != null) Console.WriteLine(e.Data);
        };
        p.BeginOutputReadLine();
        p.BeginErrorReadLine();

        return p;
    }

    protected static void Run(string file, params string[] args)
    {
        var p = StartProcess(file, args);
        p.WaitForExit();
        
        if (p.ExitCode != 0)
        {
            throw new Exception($"Non-zero exit code for {file}: ");
        }
    }
}