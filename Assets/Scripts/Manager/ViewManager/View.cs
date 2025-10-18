using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

public abstract class View : ZenAutoInjecter, IView
{
  private readonly List<IDisposable> _registerDisposables = new();

  [Inject] protected DiContainer DiContainer { get; set; }
  [Inject] private IMessageManager MessageManager { get; set; }
  [Inject] private IPoolManager PoolManager { get; set; }
  [Inject] private ILogManager LogManager { get; set; }
  [Inject] private IResourceManager ResourceManager { get; set; }
  [Inject] protected UIManager UIManager { get; set; }

  private readonly Dictionary<int, string> _idMapPath = new();

  private readonly Dictionary<string, Sprite> _spriteMap = new();
  protected override void OnAwake()
  {
    base.OnAwake();
    this.AutoFind();
    RegisterEvent();
  }
  
  private void Start() => this.OnStart();

  protected virtual void OnStart() { }

  protected virtual void RegisterEvent() { }
  
  private void AutoFind()
  {
    foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    {
      AutoFindAttribute customAttribute = property.GetCustomAttribute<AutoFindAttribute>();
      if (customAttribute != null)
      {
        string childName = string.IsNullOrEmpty(customAttribute.Value) ? property.Name : customAttribute.Value;
        Transform deepChild = this.FindDeepChild(childName);
        if ((Object) deepChild == (Object) null)
          Error($"not found {property.PropertyType.FullName} component, componentName: {childName}");
        else if (property.PropertyType == typeof (GameObject))
        {
          GameObject gameObject = deepChild.gameObject;
          property.SetValue((object) this, (object) gameObject);
        }
        else
        {
          Component component = deepChild.GetComponent(property.PropertyType);
          if ((Object) component == (Object) null && customAttribute.GetOrAdd)
            component = deepChild.gameObject.AddComponent(property.PropertyType);
          property.SetValue((object) this, (object) component);
        }
      }
    }

    BindMemberProperty();
  }

  protected virtual void BindMemberProperty()
  {
    
  }

  private Transform FindDeepChild(string childName) => FindDeepChild(this.gameObject, childName);

  private static Transform FindDeepChild(GameObject target, string childName)
  {
    Transform deepChild1 = target.transform.Find(childName);
    if ((Object) deepChild1 != (Object) null)
      return deepChild1;
    foreach (Component component in target.transform)
    {
      Transform deepChild2 = FindDeepChild(component.gameObject, childName);
      if ((Object) deepChild2 != (Object) null)
        return deepChild2;
    }
    return (Transform) null;
  }
  protected T FindDeepChild<T>(string childName) where T : Component => FindDeepChild<T>(this.gameObject, childName);
  private static T FindDeepChild<T>(GameObject target, string childName) where T : Component
  {
    Transform deepChild = FindDeepChild(target, childName);
    return !((Object) deepChild != (Object) null) ? default (T) : deepChild.gameObject.GetComponent<T>();
  }
  
  //MessageManager
  protected IDisposable Register<T>(Action<T> callback) where T : MessageModel
  {
    IDisposable disposable = this.MessageManager.Register<T>(callback);
    this._registerDisposables.Add(disposable);
    return disposable;
  }
  protected void DispatchMsg<T>(T msg) where T : MessageModel => MessageManager.DispatchMsg(msg);
  
  //PoolManager
  protected GameObject GetGameObject(string path, Action<GameObject> callback = null) => PoolManager.GetGameObject(path, callback);
  protected void ReleaseGameObject(GameObject go) => PoolManager.ReleaseGameObject(go);
  protected T GetClass<T>() where T : class, new() => PoolManager.GetClass<T>();
  protected object GetClass(Type type) => PoolManager.GetClass(type);
  protected void RecycleClass<T>(T obj) where T : class => PoolManager.RecycleClass(obj);
  
  //LogManager
  protected void Debug(string msg) => LogManager.Debug(msg);
  protected void Error(string msg) => LogManager.Error(msg);
  protected void Error(Exception e) => LogManager.Error(e);
  
  //UIManager
  protected Panel ShowUI<T>(PanelLayerType layerType = PanelLayerType.MidGround) where T : Panel => UIManager.ShowUI<T>(layerType);
  protected void HideUI<T>() where T : Panel => UIManager.HideUI<T>();
  protected void CloseUI<T>() where T : Panel => UIManager.CloseUI<T>();
  
  private List<View> Childs = new();

  public virtual void OnUpdate(float dt)
  {
    foreach (var child in Childs)
    {
      child.OnUpdate(dt);
    }
  }

  public void SetActive(bool state) => transform.gameObject.SetActive(state);

  protected T CreateUIComponentByType<T>(Transform parent) where T : UIComponent
  {
    var path = GetUIComponentPath<T>();
    var go = GetGameObject(path, go =>
    {
      go.transform.SetParent(parent);
      go.transform.localPosition = Vector3.zero;
    });
    T component = go.GetOrAddComponent<T>();
    Childs.Add(component);
    _idMapPath.TryAdd(go.GetInstanceID(), path);
    return component;
  }

  private string GetUIComponentPath<T>() where T : UIComponent
  {
    return$"Assets/GameResource/Prefab/UI/{typeof(T).Name}";
  }

  protected void CreateUIComponent<T>(List<T> list, int count, Transform parent, GameObject item = null) where T : UIComponent
  {
    if (list.Count > count)
    {
      for (int i = 0; i < list.Count; i++)
      {
        list[i].gameObject.SetActive(i < count);
        if (i < count)
        {
          list[i].transform.SetParent(parent);
        }
      }
    }
    else
    {
      for (int i = 0; i < count; i++)
      {
        if (i < list.Count)
        {
          list[i].gameObject.SetActive(true);
          list[i].transform.SetParent(parent);
        }
        else
        {
          GameObject go;
          if (item == null)
          {
            var path = GetUIComponentPath<T>();
            go = PoolManager.GetGameObject(path, obj =>
            {
              obj.transform.SetParent(parent);
            });
          }
          else
          {
            go = Instantiate(item, parent);
          }
          var component = go.GetOrAddComponent<T>();
          list.Add(component);
          Childs.Add(component);
        }
      }
    }
  }
  
  protected virtual void OnDestroy()
  {
    foreach (IDisposable registerDisposable in this._registerDisposables)
      registerDisposable.Dispose();
    this._registerDisposables.Clear();

    foreach (var child in Childs)
    {
      ReleaseGameObject(child.gameObject);
    }
    Childs.Clear();
  }

  protected void SetSprite(Image image, string spritePath, bool setNative = false)
  {
    if (!_spriteMap.TryGetValue(spritePath, out var sprite))
    {
      sprite = ResourceManager.Load<Sprite>(spritePath);
      _spriteMap.Add(spritePath, sprite);
    }
    image.sprite = sprite;
    if (setNative)
    {
      image.SetNativeSize();
    }
  }
}
