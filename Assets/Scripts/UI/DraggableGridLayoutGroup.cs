using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DraggableGridLayoutGroup : GridLayoutGroup
{
    [SerializeField] private List<Transform> orderedChildren = new List<Transform>();
    
    protected override void Start()
    {
        base.Start();
        UpdateOrderedChildren();
    }

    protected override void OnTransformChildrenChanged()
    {
        base.OnTransformChildrenChanged();
        UpdateOrderedChildren();
    }

    private void UpdateOrderedChildren()
    {
        orderedChildren.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            orderedChildren.Add(transform.GetChild(i));
        }
    }

    public void RebuildLayout()
    {
        // 按照当前子对象顺序重新布局
        for (int i = 0; i < orderedChildren.Count; i++)
        {
            if (orderedChildren[i] != null)
            {
                orderedChildren[i].SetSiblingIndex(i);
            }
        }
        SetLayoutHorizontal();
        SetLayoutVertical();
    }

    public List<Transform> GetOrderedChildren()
    {
        return new List<Transform>(orderedChildren);
    }
}