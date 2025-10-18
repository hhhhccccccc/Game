using System;
using System.Collections.Generic;

public class MessageTracker<T> : IObservable<T>
{
    private List<IObserver<T>> observers = new ();

    public IDisposable Subscribe(IObserver<T> observer)
    {
        if (!observers.Contains(observer))
        {
            this.observers.Add(observer);
        }

        return new Unsubscriber<T>(this.observers, observer);
    }
    
    private class Unsubscriber<TT> : IDisposable
    {
        private List<IObserver<TT>> _observers;
        private IObserver<TT> _observer;

        public Unsubscriber(List<IObserver<TT>> observers, IObserver<TT> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    public void OnNext(T msg)
    {
        foreach (var observer in this.observers)
        {
            observer.OnNext(msg);
        }
    }
}

