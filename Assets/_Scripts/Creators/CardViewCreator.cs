using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;

    public CardView CreateCardView(CardData cardData, Vector3 position, Quaternion rotation)
    {
        Transform parent = GameObject.Find("--- VIEWS ---").transform;
        CardView cardView = Instantiate(cardViewPrefab, position, rotation, parent);

        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, 0.25f);

        cardView.Setup(cardData);
        return cardView;
    }
}