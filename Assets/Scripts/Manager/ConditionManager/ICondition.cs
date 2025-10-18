
public abstract class ICondition
{
    public bool Check()
    {
        return OnCheck();
    }

    protected abstract bool OnCheck();
}
