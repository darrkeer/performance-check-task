using System.Runtime.Versioning;

public abstract class HookedLauncher
{
    protected string gameId;


    public HookedLauncher(string id)
    {
        gameId = id;
    }

    public abstract void Launch(Utils utils);
}