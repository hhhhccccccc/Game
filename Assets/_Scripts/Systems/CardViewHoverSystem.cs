using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView cardViewHover;

    public void Show(CardData data, Vector3 pos)
    {
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(data);
        cardViewHover.transform.position = pos;
    }

    public void Hide()
    {
        cardViewHover.gameObject.SetActive(false);
    }
}