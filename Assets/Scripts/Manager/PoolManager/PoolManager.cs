using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class PoolManager : ManagerBase, IPoolManager
{
    [Inject] private IResourceManager ResourceManager { get; set; }

    private Dictionary<string, Queue<GameObject>> _gameObjectPool;
    private Transform BattlePoolRoot { get; set; }
    public bool Initiated { get; set; }

    private readonly Dictionary<int, string> _idMapPath = new Dictionary<int, string>();
    
    protected override IEnumerator OnInit()
    {
        _gameObjectPool = new Dictionary<string, Queue<GameObject>>();
        this.BattlePoolRoot = new GameObject("[BattleRoot]").transform;
        BattlePoolRoot.gameObject.SetActive(false);
        
        _classPool = new Dictionary<Type, Queue<object>>();
        this.Initiated = true;
        yield break;
    }
    
    public GameObject GetGameObject(string path, Action<GameObject> callback = null)
    {
        Queue<GameObject> source;
        GameObject prefab;
        if (!this._gameObjectPool.TryGetValue(path, out source) || !source.Any())
        {
            prefab = Object.Instantiate<GameObject>(
                    this.ResourceManager.Load<GameObject>(path));
        }
        else
        {
            prefab = source.Dequeue();
        }

        callback?.Invoke(prefab);
        _idMapPath[prefab.GetInstanceID()] = path;
        return prefab;
    }

    public void ReleaseGameObject(GameObject go)
    {
        var instanceID = go.GetInstanceID();
        if (_idMapPath.TryGetValue(instanceID, out var path))
        {
            this._idMapPath.Remove(instanceID);
            go.transform.position = new Vector3(1000, 1000);
            Queue<GameObject> source;
            if (!this._gameObjectPool.TryGetValue(path,out source))
            {
                source=new Queue<GameObject>();
                this._gameObjectPool.Add(path,source);
            }
            source.Enqueue(go);
        }
        else
        {
            Object.Destroy(go);
        }
    }
    
    private Dictionary<Type, Queue<object>> _classPool; 

    [Inject] private DiContainer DiContainer;
    
    public T GetClass<T>() where T : class, new()
    {
        var type = typeof(T);
        if (!_classPool.TryGetValue(type, out var queue))
        {
            queue = new Queue<object>();
            _classPool[type] = queue;
        }

        T model;
        
        if (queue.Count > 0)
        {
            model = (T)queue.Dequeue();
        }
        else
        {
            model = DiContainer.Resolve<T>();
        }

        if (model is IAlloc alloc)
        {
            alloc.Alloc();
        }

        return model;
    }

    public object GetClass(Type type)
    {
        if (!_classPool.TryGetValue(type, out var queue))
        {
            queue = new Queue<object>();
            _classPool[type] = queue;
        }

        object model;

        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        if (queue.Count > 0)
        {
            model = queue.Dequeue();
        }
        else
        {
            model = DiContainer.Resolve(type);
        }

        if (model is IAlloc alloc)
        {
            alloc.Alloc();
        }

        return model;
    }

    public void RecycleClass<T>(T obj) where T : class
    {
        if (obj == null) return;

        var type = typeof(T);
        if (!_classPool.TryGetValue(type, out var queue))
        {
            queue = new Queue<object>();
            _classPool[type] = queue;
        }

        if (obj is IRecycle recycle)
        {
            recycle.Recycle();
        }

        queue.Enqueue(obj);
    }
    
    public void ClearGameObjectPool()
    {
        foreach (var queue in _classPool.Values)
        {
            queue.Clear();
        }
        _classPool.Clear();
    }

    public void ClearClassPool()
    {
        _gameObjectPool.Clear();
    }
}