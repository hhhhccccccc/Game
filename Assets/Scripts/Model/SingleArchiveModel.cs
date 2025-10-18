using System;
using System.Collections;
using Zenject;

public abstract class SingleArchiveModel : ISingleArchiveModel
{
    [Inject] protected DiContainer DiContainer { get; set; }
    [Inject] private IArchiveManager ArchiveManager { get; set; }
    [Inject] protected ConfigManager ConfigManager { get; set; } 
    [Inject] protected ILogManager LogManager { get; set; } 
    [Inject] private IMessageManager MessageManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    protected void Debug(string msg) => LogManager.Debug(msg);
    protected void Error(string msg) => LogManager.Error(msg);
    protected IDisposable Register<T>(Action<T> action) where T : MessageModel => MessageManager.Register<T>(action);
    protected void Dispatch<T>(T model) where T : MessageModel => MessageManager.DispatchMsg(model);
    protected T GetClass<T>() where T : class, new() => PoolManager.GetClass<T>();
    protected object GetClass(Type type) => PoolManager.GetClass(type);
    protected void RecycleClass<T>(T obj) where T : class => PoolManager.RecycleClass(obj);
    public virtual void Init()
    {
        
    }
    
    public void Save()
    {
        int? hashCode = this.GetType()?.FullName?.GetHashCode();
        if (!hashCode.HasValue)
            return;
        this.ArchiveManager.Save(hashCode.ToString(), (object) this);
    }
}