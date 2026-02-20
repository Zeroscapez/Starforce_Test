using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleCardUI : MonoBehaviour
{
    // Add public property to access the card data
    public BattleCard CurrentCard { get; private set; }

    
    [Header("References")]
    public Image colorIndicator;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI cardNameText;
    public Image cardIcon;
    public Button selectButton;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(BattleCard card, System.Action<BattleCard> onClick)
    {
        CurrentCard = card; // Set the property instead of private field

        cardIcon.sprite = card.icon;
        cardNameText.text = card.cardName;
        damageText.text = card.damage.ToString();
        colorIndicator.color = card.colorIndicator;

        rectTransform.sizeDelta *= card.sizeMultiplier;
        selectButton.onClick.AddListener(() => onClick?.Invoke(card));
    }

    public void SetInteractable(bool state)
    {
        selectButton.interactable = state;
    }
}