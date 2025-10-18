
using System;
using System.Collections.Generic;
using Zenject;

public abstract class SingleModel : IModel, ISingleModel
{
    [Inject] protected DiContainer DiContainer { get; set; }
    [Inject] protected IMessageManager MessageManager { get; set; }

    private readonly List<IDisposable> _registerList = new List<IDisposable>();

    protected IDisposable Register<T>(Action<T> action) where T : MessageModel
    {
        var disposable = MessageManager.Register<T>(action);
        _registerList.Add(disposable);
        return disposable;
    }

    protected void ClearEvent()
    {
        foreach (var disposable in _registerList)
        {
            disposable.Dispose();
        }
        
        _registerList.Clear();
    }

    public virtual void Clear()
    {
        ClearEvent();
    }
}
