using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MonoComponentEx 
{
    [MenuItem("GameObject/UIFrame/Image", false, 10)]
    static void CreateImage(MenuCommand menuCommand)
    {
        // 创建游戏对象
        GameObject go = new GameObject("Img");
        go.AddComponent<CanvasRenderer>();
        var component = go.AddComponent<Image>();
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        component.raycastTarget = false;
        // 设置父级
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // 注册创建操作以便撤销
        Undo.RegisterCreatedObjectUndo(go, "Image");
        Selection.activeObject = go;
    }
    
    [MenuItem("GameObject/UIFrame/Button", false, 10)]
    static void CreateButton(MenuCommand menuCommand)
    {
        // 创建游戏对象
        GameObject btnObj = new GameObject("Btn");
        btnObj.AddComponent<CanvasRenderer>();
        var img = btnObj.AddComponent<Image>();
        img.type = Image.Type.Simple;
        var btn = btnObj.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        var nav = new Navigation();
        nav.mode = Navigation.Mode.None;
        btn.navigation = nav;
        btnObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 50);
        
        GameObject txtObj = new GameObject("Txt");
        txtObj.transform.SetParent(btnObj.transform);
        txtObj.AddComponent<CanvasRenderer>();
        var txt = txtObj.AddComponent<TextMeshProUGUI>();
        txt.fontSize = 30;
        txt.color = Color.black;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Center;
        txt.verticalAlignment = VerticalAlignmentOptions.Middle;
        txt.raycastTarget = false;
        var rect = txt.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;    // 左下角 (0,0)
        rect.anchorMax = Vector2.one;     // 右上角 (1,1)
        rect.offsetMin = Vector2.zero;    // 左边和下边的偏移
        rect.offsetMax = Vector2.zero;    // 右边和上边的偏移
        
        // 设置父级
        GameObjectUtility.SetParentAndAlign(btnObj, menuCommand.context as GameObject);
        // 注册创建操作以便撤销
        Undo.RegisterCreatedObjectUndo(btnObj, "Button");
        Selection.activeObject = btnObj;
    }
    
    [MenuItem("GameObject/UIFrame/Text", false, 10)]
    static void CreateText(MenuCommand menuCommand)
    {
        GameObject txtObj = new GameObject("Txt");
        txtObj.AddComponent<CanvasRenderer>();
        var txt = txtObj.AddComponent<TextMeshProUGUI>();
        txt.fontSize = 30;
        txt.color = Color.black;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Center;
        txt.verticalAlignment = VerticalAlignmentOptions.Middle;
        txt.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
        txt.raycastTarget = false;
        // 设置父级
        GameObjectUtility.SetParentAndAlign(txtObj, menuCommand.context as GameObject);
        // 注册创建操作以便撤销
        Undo.RegisterCreatedObjectUndo(txtObj, "Text");
        Selection.activeObject = txtObj;
    }
}
