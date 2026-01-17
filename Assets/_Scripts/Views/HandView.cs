using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class HandView : MonoBehaviour
{
    #region 卡牌布局参数
    public bool isHorizontal;
    public float maxWidth = 25f;
    public float cardSpacing = 2f;
    public Vector3 centerPoint;
    [Header("弧形参数")]
    public float maxTotalAngle = 90f;
    public float angleBetweenCards = 7f;
    public float radius = 17f;
    #endregion

    [SerializeField] private List<Vector3> cardPositions = new();
    private readonly List<Quaternion> cardRotations = new();

    private readonly List<CardView> cardViews = new();
    private const int BASE_SORTING_ORDER = 20;
    private const float ZOffset = 0.1f;

    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    public IEnumerator AddCard(CardView cardView)
    {
        cardViews.Add(cardView);
        yield return SetCardLayout(0.15f);
    }

    public CardView RemoveCard(CardData card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null) return null;
        cardViews.Remove(cardView);
        StartCoroutine(SetCardLayout(0.15f));
        return cardView;
    }

    private CardView GetCardView(CardData card)
    {
        return cardViews.FirstOrDefault(cardView => cardView.CardData == card);
    }

    private void CalculatePosition(int numberOfCards, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();

        if (numberOfCards == 0)
            return;
        if (horizontal)
        {
            float currentCardWidth = (numberOfCards - 1) * cardSpacing;
            float totalWidth = Mathf.Min(maxWidth, currentCardWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : cardSpacing;

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = -(totalWidth / 2) + i * currentSpacing;
                Vector3 pos = new(xPos, centerPoint.y, 0f);
                Quaternion rotation = Quaternion.identity;

                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            float currentTotalAngle = (numberOfCards - 1) * angleBetweenCards;
            float totalAngle = Mathf.Min(maxTotalAngle, currentTotalAngle);

            float currentAngleBetween = totalAngle > 0 ? totalAngle / (numberOfCards - 1) : angleBetweenCards;
            float cardAngle = totalAngle / 2;

            for (int i = 0; i < numberOfCards; i++)
            {
                float angle = cardAngle - i * currentAngleBetween;
                Vector3 pos = FanCardPosition(angle);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        );
    }

    public IEnumerator SetCardLayout(float duration)
    {
        for (int i = 0; i < cardViews.Count; i++)
        {
            CardView curCardView = cardViews[i];
            CardTransform cardTransform = CulAndGetCardTrans(i, cardViews);
            int sortOrder = BASE_SORTING_ORDER + i;
            curCardView.GetComponent<SortingGroup>().sortingOrder = sortOrder;
            cardTransform.pos = new Vector3(cardTransform.pos.x, cardTransform.pos.y, -ZOffset * i);
            curCardView.UpdatePositionRotation(cardTransform.pos, cardTransform.rot, duration);
        }

        yield return new WaitForSeconds(duration);
    }

    public CardTransform CulAndGetCardTrans(int cardIndex, List<CardView> cards)
    {
        int numberOfCards = cards.Count;
        CalculatePosition(numberOfCards, isHorizontal);
        return new CardTransform(cardPositions[cardIndex], cardRotations[cardIndex]);
    }
}