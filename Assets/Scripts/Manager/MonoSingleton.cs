using UnityEngine;
using Zenject;

/// <summary>
/// 泛型 MonoBehaviour 单例基类（线程安全 + 自动创建 + 跨场景持久化）
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _isApplicationQuitting = false;
    
    public static T Instance
    {
        get
        {
            if (_isApplicationQuitting)
            {
                Debug.LogWarning($"[MonoSingleton] Instance '{typeof(T)}' already destroyed on application quit.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // 查找现有实例
                    _instance = FindObjectOfType<T>();

                    // 如果没找到，自动创建一个新 GameObject
                    if (_instance == null)
                    {
                        var singletonName = $"[{typeof(T).Name}]";
                        var singletonObject = new GameObject(singletonName);
                        _instance = singletonObject.AddComponent<T>();

                        // 使单例跨场景持久化（可选）
                        DontDestroyOnLoad(singletonObject);

                        Debug.Log($"[MonoSingleton] An instance of {typeof(T)} was auto-created: {singletonObject.name}");
                    }
                    else
                    {
                        // 确保只有一个实例
                        Debug.Log($"[MonoSingleton] Using existing instance: {_instance.gameObject.name}");
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        // 防止重复实例化
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"[MonoSingleton] Duplicate instance of {typeof(T)} detected. Destroying {gameObject.name}.");
            Destroy(gameObject);
            return;
        }

        // 初始化单例
        _instance = (T)this;
        DontDestroyOnLoad(gameObject); // 跨场景持久化（可选）
    }

    public virtual void SingletonInit(DiContainer diContainer)
    {
        
    }

    protected virtual void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
    }
}