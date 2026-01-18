using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] private Transform cardContainer; // 用于放置卡牌的容器
    [SerializeField] private CardUI cardUIPrefab; // 卡牌UI预制体
    [SerializeField] private LayoutGroup layoutGroup; // 布局组件

    private List<CardData> cardLibraryData = new List<CardData>(); // 存储卡牌数据
    private List<CardUI> cardUIInstances = new List<CardUI>(); // 存储实例化的卡牌UI
    
    public void Initialize(List<CardData> initialCards = null)
    {
        if(initialCards != null)
        {
            cardLibraryData = new List<CardData>(initialCards);
        }
        
        RefreshDisplay();
    }
    
    public void AddCard(CardData cardData)
    {
        if(cardData != null)
        {
            cardLibraryData.Add(cardData);
            CreateCardUI(cardData);
        }
    }
    
    public void RemoveCard(CardData cardData)
    {
        if(cardData != null)
        {
            int index = cardLibraryData.IndexOf(cardData);
            if(index >= 0)
            {
                cardLibraryData.RemoveAt(index);
                
                // 销毁对应的UI实例
                if(index < cardUIInstances.Count && cardUIInstances[index] != null)
                {
                    DestroyImmediate(cardUIInstances[index].gameObject);
                    cardUIInstances.RemoveAt(index);
                }
            }
        }
    }
    
    public void RemoveCardAt(int index)
    {
        if(index >= 0 && index < cardLibraryData.Count)
        {
            cardLibraryData.RemoveAt(index);
            
            // 销毁对应的UI实例
            if(index < cardUIInstances.Count && cardUIInstances[index] != null)
            {
                DestroyImmediate(cardUIInstances[index].gameObject);
                cardUIInstances.RemoveAt(index);
            }
        }
    }
    
    public void MoveCard(int fromIndex, int toIndex)
    {
        if(fromIndex >= 0 && fromIndex < cardLibraryData.Count &&
           toIndex >= 0 && toIndex < cardLibraryData.Count &&
           fromIndex != toIndex)
        {
            // 移动数据
            CardData cardToMove = cardLibraryData[fromIndex];
            cardLibraryData.RemoveAt(fromIndex);
            
            if(toIndex > fromIndex)
            {
                cardLibraryData.Insert(toIndex, cardToMove);
            }
            else
            {
                cardLibraryData.Insert(toIndex, cardToMove);
            }
            
            // 移动UI实例
            if(fromIndex < cardUIInstances.Count && toIndex < cardUIInstances.Count)
            {
                CardUI uiToMove = cardUIInstances[fromIndex];
                cardUIInstances.RemoveAt(fromIndex);
                
                if(toIndex > fromIndex)
                {
                    cardUIInstances.Insert(toIndex, uiToMove);
                }
                else
                {
                    cardUIInstances.Insert(toIndex, uiToMove);
                }
                
                // 更新UI的兄弟节点索引
                for(int i = 0; i < cardUIInstances.Count; i++)
                {
                    if(cardUIInstances[i] != null)
                    {
                        cardUIInstances[i].transform.SetSiblingIndex(i);
                    }
                }
            }
        }
    }
    
    public List<CardData> GetOrderedCardList()
    {
        return new List<CardData>(cardLibraryData);
    }
    
    private void CreateCardUI(CardData cardData)
    {
        if(cardUIPrefab != null && cardContainer != null)
        {
            CardUI cardUI = Instantiate(cardUIPrefab, cardContainer);
            cardUI.Setup(cardData);
            cardUIInstances.Add(cardUI);
        }
    }
    
    public void RefreshDisplay()
    {
        // 清除现有的卡牌UI实例
        foreach(CardUI cardUI in cardUIInstances)
        {
            if(cardUI != null)
            {
                DestroyImmediate(cardUI.gameObject);
            }
        }
        cardUIInstances.Clear();
        
        // 重新创建所有卡牌UI
        foreach(CardData cardData in cardLibraryData)
        {
            CreateCardUI(cardData);
        }
    }
    
    public void OnCardOrderChanged()
    {
        // 通过比较当前UI实例的Sibling Index来更新数据顺序
        ReorderBasedOnUI();
    }
    
    private void ReorderBasedOnUI()
    {
        // 首先根据当前的UI层级顺序重新排列数据
        Dictionary<int, CardData> orderedDataMap = new Dictionary<int, CardData>();
        
        for(int i = 0; i < cardContainer.childCount; i++)
        {
            Transform child = cardContainer.GetChild(i);
            CardUI cardUI = child.GetComponent<CardUI>();
            
            if(cardUI != null)
            {
                // 找到该UI实例在原列表中的对应数据
                int dataIndex = cardLibraryData.IndexOf(cardUI.CardData);
                if(dataIndex != -1)
                {
                    orderedDataMap[i] = cardLibraryData[dataIndex];
                }
            }
        }
        
        // 根据映射重建数据列表
        List<CardData> newDataList = new List<CardData>();
        for(int i = 0; i < cardContainer.childCount; i++)
        {
            if(orderedDataMap.ContainsKey(i))
            {
                newDataList.Add(orderedDataMap[i]);
            }
        }
        
        cardLibraryData = newDataList;
    }
}