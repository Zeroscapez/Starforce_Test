using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public BattleCard battleCard;
    public Image iconImage;
    public Image background; // Used to display the card color

    private BattleCardManager cardManager;

    void Start()
    {
        cardManager = FindObjectOfType<BattleCardManager>();
        SetupCard();
    }

    void SetupCard()
    {
        if (battleCard)
        {
            iconImage.sprite = battleCard.icon;
            background.color = battleCard.colorIndicator;
            // Ensure the card maintains a consistent size. 
            // For now, we'll assume localScale is (1,1,1) and we control size via RectTransform.
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       // Debug.Log("Clicked: " + battleCard.cardName);
        cardManager.HandleCardClick(this);
    }
}
