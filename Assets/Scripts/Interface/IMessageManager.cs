using System;

public interface IMessageManager : IManager
{
    IDisposable RegisterController<T>(Type t, Action<T> callback) where T : MessageModel;
    
    IDisposable Register<T>(Action<T> callback) where T : MessageModel;

    void DispatchMsg<T>(T msg) where T : MessageModel;

    void Clear();
}
