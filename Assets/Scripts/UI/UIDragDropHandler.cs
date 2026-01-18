using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    [SerializeField] private CardLibrary cardLibrary;
    
    public void OnDrop(PointerEventData eventData)
    {
        // 处理放置操作
        CardUI droppedCard = eventData.pointerDrag.GetComponent<CardUI>();
        if (droppedCard != null)
        {
            // 将卡牌移动到当前位置
            RectTransform dropRectTransform = droppedCard.GetComponent<RectTransform>();
            dropRectTransform.SetParent(transform as RectTransform, false);
            
            // 通知卡牌库更新顺序
            if (cardLibrary != null)
            {
                cardLibrary.OnCardOrderChanged();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 当拖拽的物体进入此区域时
        CardUI draggedCard = eventData.pointerDrag.GetComponent<CardUI>();
        if (draggedCard != null && draggedCard.transform.parent != transform)
        {
            // 将被拖拽的卡牌移到当前位置
            RectTransform dragRectTransform = draggedCard.GetComponent<RectTransform>();
            dragRectTransform.SetParent(transform as RectTransform, false);
            dragRectTransform.SetAsLastSibling(); // 确保它显示在最前
            
            if (cardLibrary != null)
            {
                cardLibrary.OnCardOrderChanged();
            }
        }
    }
}