using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    private readonly List<CardData> drawPile = new();
    private readonly List<CardData> discardPile = new();
    private readonly List<CardData> handPile = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    public void Setup(List<CardConfig> deckConfig)
    {
        foreach (CardConfig config in deckConfig)
        {
            CardData cardData = new(config);
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
        drawPile.AddRange(discardPile);
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
        SpendManaGA spendManaGA = new(playCardGA.CardData.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);
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
}