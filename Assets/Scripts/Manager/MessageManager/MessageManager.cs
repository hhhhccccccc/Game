using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MessageManager : ManagerBase, IMessageManager
{
    private Dictionary<Type, MessageTracker<MessageModel>> _msgs;
    
    protected override IEnumerator OnInit()
    {
        _msgs = new();
        yield break;
    }

    public IDisposable RegisterController<T>(Type t, Action<T> callback) where T : MessageModel
    {
        if (!this._msgs.TryGetValue(t, out var msgType))
        {
            msgType = new MessageTracker<MessageModel>();
            this._msgs.Add(t, msgType);
        }
        
        MessageReporter<MessageModel> report = new MessageReporter<MessageModel>();
        return report.Subscribe(msgType, o => callback((T)o));
    }
    
    public IDisposable Register<T>(Action<T> callback) where T : MessageModel
    {
        if (!this._msgs.TryGetValue(typeof(T), out var msgType))
        {
            msgType = new MessageTracker<MessageModel>();
            this._msgs.Add(typeof(T), msgType);
        }
        
        MessageReporter<MessageModel> report = new MessageReporter<MessageModel>();
        return report.Subscribe(msgType, o => callback((T)o));
    }

    public void DispatchMsg<T>(T msg) where T : MessageModel
    {
        if (this._msgs.TryGetValue(typeof(T), out var msgType))
        {
            msgType.OnNext(msg);
        }
    }

    public void Clear()
    {
        _msgs.Clear();
    }
}
