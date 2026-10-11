using System.Runtime.Versioning;

public abstract class HookedLauncher
{
    protected string GameId { get; private set; }

    public HookedLauncher(string id)
    {
        GameId = id;
    }

    public abstract void Launch(Utils utils);
}