using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text desc;
    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text rarity;
    
    public CardData CardData { get; private set; }
    private Canvas canvas;
    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private Vector3 originPos;
    private Transform originParent;
    private int originSiblingIndex;
    
    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void Setup(CardData cardData)
    {
        CardData = cardData;
        
        if(title != null) title.text = cardData?.Name ?? "No Title";
        if(desc != null) 
        {
            string desc = cardData?.Desc ?? "No Description";
            if (cardData.CDTurns > 0)
                desc += $"，CD: {cardData.CDTurns}";
            if (cardData.IsEcho)
                desc += "，回响";
            this.desc.text = desc;
        }
        if(type != null) type.text = cardData?.Type ?? "No Type";
        
        if(rarity != null)
        {
            rarity.text = cardData?.Rarity switch
            {
                "1级：普通卡" => "普通",
                "2级：稀有卡" => "稀有",
                "3级：超级卡" => "超级",
                "4级：传说卡" => "传说",
                _ => "普通"
            };

            rarity.color = cardData.Rarity switch
            {
                "1级：普通卡" => Color.gray,
                "2级：稀有卡" => Color.blue,
                "3级：超级卡" => Color.magenta,
                "4级：传说卡" => Color.yellow,
                _ => Color.white
            };
        }
        
        if(image != null && cardData?.Image != null) 
        {
            image.sprite = cardData.Image;
            image.preserveAspect = true;
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 保存初始状态
        originPos = rectTrans.anchoredPosition;
        originParent = transform.parent;
        originSiblingIndex = transform.GetSiblingIndex();
        
        // 启用CanvasGroup的阻止交互，这样可以保持在顶层但不影响事件传递
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        
        Debug.Log("开始拖拽卡牌");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), 
            eventData.position, 
            eventData.pressEventCamera, 
            out position
        );
        
        rectTrans.anchoredPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originParent, false); // 不改变位置，只改变父对象
        transform.SetSiblingIndex(originSiblingIndex);
        
        // 恢复CanvasGroup设置
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        
        // 触发重新排序事件
        if(transform.parent != null)
        {
            CardLibrary library = transform.parent.GetComponentInParent<CardLibrary>();
            if(library != null)
                library.OnCardOrderChanged();
        }
        
        Debug.Log("结束拖拽卡牌");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"点击了卡牌: {CardData?.Name}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"释放了卡牌: {CardData?.Name}");
    }
    
    private T FindInParents<T>(GameObject obj) where T : Component
    {
        Transform tr = obj.transform;
        while(tr != null)
        {
            T result = tr.GetComponent<T>();
            if(result != null) return result;
            tr = tr.parent;
        }
        return null;
    }
}