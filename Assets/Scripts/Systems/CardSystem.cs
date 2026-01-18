using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    [ShowInInspector] private readonly List<CardData> drawPile = new();
    [ShowInInspector] private readonly List<CardData> discardPile = new();
    [ShowInInspector] private readonly List<CardData> handPile = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.AttachPerformer<EchoCardsGA>(EchoCardsPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.DetachPerformer<EchoCardsGA>();
    }

    #region 旧方法
    public void Setup(HeroConfig heroConfig)
    {
        foreach (CardConfig cardConfig in heroConfig.Deck)
        {
            CardData cardData = new(cardConfig);
            drawPile.Add(cardData);
        }
    }
    #endregion

    public void Setup(CardInfoContainer cardInfoContainer)
    {
        foreach (KeyValuePair<int, CardInfo> dic in cardInfoContainer.dataDic)
        {
            CardData cardData = new(dic.Value);
            drawPile.Add(cardData);
        }
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        int notDrawAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
            yield return DrawCard();
        if (notDrawAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawAmount; i++)
                yield return DrawCard();
        }
    }

    private IEnumerator DrawCard()
    {
        CardData card = drawPile.Draw();
        handPile.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        // drawPile.AddRange(discardPile);
        discardPile.Clear();
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach (CardData card in handPile)
        {
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }

        handPile.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        handPile.Remove(playCardGA.CardData);
        CardView cardView = handView.RemoveCard(playCardGA.CardData);
        yield return DiscardCard(cardView);
        if (playCardGA.CardData.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGA = new(playCardGA.CardData.ManualTargetEffect, new List<CombatantView>
            {
                playCardGA.ManualTarget
            });
            ActionSystem.Instance.AddReaction(performEffectGA);
        }

        foreach (AutoTargetEffect effectWrapper in playCardGA.CardData.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGA performEffectGA = new(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.CardData);
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

    private IEnumerator EchoCardsPerformer(EchoCardsGA echoCardsGA)
    {
        for (int i = 0; i < echoCardsGA.EchoAmount; i++)
        {
            drawPile.Add(new CardData(echoCardsGA.CardConfig));
            CardView cardView = CardViewCreator.Instance.CreateCardView(new CardData(echoCardsGA.CardConfig), Vector3.zero, Quaternion.identity);
            yield return EchoPerformance(cardView);
        }
    }

    public IEnumerator EchoPerformance(CardView cardView)
    {
        Tween tween = cardView.transform.DOMove(drawPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}