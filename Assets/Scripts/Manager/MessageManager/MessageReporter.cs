using System;
public class MessageReporter<T> : IObserver<T>
{
    private IDisposable _unsubscriber { get; set; }

    private Action<T> _action { get; set; }

    public IDisposable Subscribe(IObservable<T> provider, Action<T> action)
    {
        if (provider == null)
        {
            return null;
        }

        this._unsubscriber = provider.Subscribe(this);
        this._action = action;

        return this._unsubscriber;
    }

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(T value)
    {
        this._action.Invoke(value);
    }

    public void UnSubscribe()
    {
        this._unsubscriber.Dispose();
    }
}
