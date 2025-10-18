using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

public class PanelLayer
{
  private readonly List<Panel> _hidePanel = new();
  private readonly List<Panel> _openPanel = new();
  private readonly Dictionary<string, Panel> _panelMap = new();

  [Inject]
  private IResourceManager ResourceManager { get; set; }

  [Inject]
  private ViewManager ViewManager { get; set; }
  [Inject]
  private DiContainer DiContainer { get; set; }

  public Canvas Canvas { get; set; }

  private const int SortingLayerBase = 0;
  private const int SortingLayerDelta = 50;

  public void Init(PanelLayerType layerType)
  {
    GameObject gameObject = new GameObject($"[{layerType} Layer]");
    gameObject.transform.SetParent(this.ViewManager.UIRoot);
    this.Canvas = gameObject.AddComponent<Canvas>();
    this.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    Canvas canvas = this.Canvas;
    canvas.sortingOrder = (int)layerType * 100;
    this.Canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
    CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    canvasScaler.referenceResolution = new Vector2((float) GameConst.ReferenceResolutionX, (float) GameConst.ReferenceResolutionY);
    canvasScaler.matchWidthOrHeight = GameConst.MatchWidthOrHeight;
    gameObject.AddComponent<GraphicRaycaster>();
  }

  public Panel ShowUI<T>() where T : Panel
    {
      Panel panel;
      if (!this._panelMap.TryGetValue(typeof(T).Name, out panel))
      {
        var obj = Object.Instantiate<GameObject>(this.ResourceManager.Load<GameObject>($"Assets/GameResource/Prefab/UI/{typeof(T).Name}"), this.Canvas.transform);
        if (obj.GetComponent<T>() == null)
        {
            obj.AddComponent<T>();
        }
        panel = obj.GetComponent<Panel>();
        this._openPanel.Add(panel);
        this._panelMap[typeof(T).Name] = panel;
      }
      
      if (this._hidePanel.Contains(panel))
      {
        this._hidePanel.Remove(panel);
        this._openPanel.Add(panel);
      }
      panel.transform.SetAsLastSibling();
      panel.gameObject.SetActive(true);
      panel.OnShow();
      return panel;
  }

  public T GetUI<T>() where T : Panel
  {
    if (this._panelMap.TryGetValue(typeof(T).Name, out var panel))
      return panel as T;
    throw new Exception("Get panel error, not found panel: " + typeof (T).FullName);
  }

  public void HideUI<T>() where T : Panel
  {
    if (!this._panelMap.TryGetValue(typeof(T).Name, out var panel))
      return;
    panel.OnHide();
    this._openPanel.Remove(panel);
    panel.gameObject.SetActive(false);
    panel.transform.SetAsFirstSibling();
    this._hidePanel.Add(panel);
  }
  
  public void CloseUI<T>() where T : Panel
  {
    if (!this._panelMap.TryGetValue(typeof(T).Name, out var panel))
      return;
    this._openPanel.Remove(panel);
    this._panelMap.Remove(typeof(T).Name);
    Object.Destroy((Object) panel.gameObject);
  }

  public void CloseAllUI()
  {
    foreach (Panel panel in this._openPanel)
    {
        Object.Destroy((Object)panel.gameObject);
    }
    this._openPanel.Clear();
    this._panelMap.Clear();
    }

  public void HideAllUI()
  {
    foreach (Panel panel in this._openPanel)
    {
        panel.OnHide();
        panel.gameObject.SetActive(false);
        panel.transform.SetAsFirstSibling();
        this._hidePanel.Add(panel);
    }
    this._openPanel.Clear();
  }

  public void ShowAllUI()
  {
    foreach (Panel panel in this._hidePanel.ToList<Panel>())
    {
      if (this._hidePanel.Contains(panel))
      {
        this._openPanel.Add(panel);
      }
      panel.transform.SetAsLastSibling();
      panel.gameObject.SetActive(true);
      panel.OnShow();
    }
    this._hidePanel.Clear();
  }

  public void OnUpdate(float dt)
  {
    foreach (var (name, panel) in _panelMap)
    {
      panel.OnUpdate(dt);
    }
  }
}
