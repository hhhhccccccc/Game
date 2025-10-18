using System.Collections;

public abstract class ManagerBase : IManager
{
    public IEnumerator Init()
    {
        yield return OnInit();
    }
    
    protected virtual IEnumerator OnInit()
    {
        yield break;
    }
}
