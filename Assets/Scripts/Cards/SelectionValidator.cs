using System.Collections.Generic;
using UnityEngine;

public class SelectionValidator : MonoBehaviour
{
    public List<BattleCardUI> GetValidSelections(
        BattleCardUI selectedCard,
        List<BattleCardUI> allCards
    )
    {
        List<BattleCardUI> validCards = new List<BattleCardUI>();

        if (selectedCard.CurrentCard.isWhiteCard)
            return allCards;

        foreach (BattleCardUI card in allCards)
        {
            if (IsValidSelection(selectedCard, card, allCards))
                validCards.Add(card);
        }

        return validCards;
    }

    private bool IsValidSelection(
        BattleCardUI source,
        BattleCardUI target,
        List<BattleCardUI> hand
    )
    {
        if (source.CurrentCard.cardName == target.CurrentCard.cardName)
            return true;

        Vector2Int sourcePos = GetGridPosition(source, hand);
        Vector2Int targetPos = GetGridPosition(target, hand);

        return sourcePos.x == targetPos.x || sourcePos.y == targetPos.y;
    }

    private Vector2Int GetGridPosition(BattleCardUI card, List<BattleCardUI> hand)
    {
        int index = hand.IndexOf(card);
        return new Vector2Int(index % 3, index / 3);
    }
}