using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : ManagerBase, IInitRootAfter, IUpdate
{
  private readonly Dictionary<PanelLayerType, PanelLayer> _panelLayers = new();

  [Inject]
  private ViewManager ViewManager { get; set; }

  [Inject]
  private DiContainer DiContainer { get; set; }

  private Dictionary<Type, int> LayerToType = new();
  private PanelLayerType GetLayerType<T>() => (PanelLayerType)LayerToType.GetValueOrDefault(typeof(T), 1);

  private void AddLayerToType<T>(PanelLayerType layerType)
  {
    LayerToType[typeof(T)] = (int)layerType;
  }
  
  protected override IEnumerator OnInit()
  {
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    while (!this.ViewManager.Initiated)
      yield return (object) endOfFrame;
    foreach (PanelLayerType layerType in Enum.GetValues(typeof (PanelLayerType)))
      this.GetLayer(layerType);
  }

  public PanelLayer GetLayer(PanelLayerType layerType)
  {
    PanelLayer layer1;
    if (this._panelLayers.TryGetValue(layerType, out layer1))
      return layer1;
    PanelLayer layer2 = new PanelLayer();
    this.DiContainer.Inject((object) layer2);
    layer2.Init(layerType);
    this._panelLayers.Add(layerType, layer2);
    this.DiContainer.Bind<PanelLayer>().WithId((object) layerType).FromInstance(layer2).AsTransient();
    return layer2;
  }

  public T GetUI<T>() where T : Panel
  {
    var layerType = GetLayerType<T>();
    var ui = this.GetLayer(layerType).GetUI<T>();
    return ui;
  }
  
  public Panel ShowUI<T>(PanelLayerType layerType = PanelLayerType.MidGround) where T : Panel 
  {
      var ui = this.GetLayer(layerType).ShowUI<T>();
      AddLayerToType<T>(layerType);
      return ui;
  }

  public void HideUI<T>() where T : Panel
  {
    var layerType = GetLayerType<T>();
    this.GetLayer(layerType).HideUI<T>();
  }

  public void CloseUI<T>() where T : Panel
  {
    var layerType = GetLayerType<T>();
    this.GetLayer(layerType).CloseUI<T>();
  }
  
  public void ShowAllUI(PanelLayerType layerType = PanelLayerType.MidGround)
  {
    GetLayer(layerType).ShowAllUI();
  }

  public void HideAllUI(PanelLayerType layerType = PanelLayerType.MidGround)
    {
    GetLayer(layerType).ShowAllUI();
  }
  
  public void CloseAllUI(PanelLayerType layerType = PanelLayerType.MidGround)
    {
    GetLayer(layerType).CloseAllUI();
  }
  public void ShowAllUI()
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.ShowAllUI();
    }
  }

  public void HideAllUI()
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.HideAllUI();
    }
  }
  
  public void CloseAllUI()
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.CloseAllUI();
    }
  }

  public void OnUpdate(float dt)
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.OnUpdate(dt);
    }
  }
  
  public Vector2 ConvertWorldToUIPosition(Vector3 worldPosition, RectTransform rectTransform)
  {
    // 将世界坐标转换为屏幕坐标
    Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

    // 将屏幕坐标转换为UI局部坐标
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
      rectTransform, 
      screenPoint, 
      null, // Overlay模式相机为null
      out localPoint
    );

    return localPoint;
  }
}
