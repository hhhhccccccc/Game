public abstract class MessageModel : IRecycle
{
    public virtual void Recycle()
    {
        
    }
}

public interface IAlloc
{
    public void Alloc();
}

public interface IRecycle
{
    public void Recycle();
}