using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public BattleCard battleCard;
    public Image iconImage;
    public Image gradeIndicator; // Used to display the card color
    public GameObject visualElement;
    public int Row { get; private set; }// Row position on the grid, set by CardManager when instantiated
    public int Column { get; private set; } // Column position on the grid, set by CardManager when instantiated

    void Start()
    {
    
        SetupCard();

    }

    void SetupCard()
    {
        if (battleCard != null)
        {
            iconImage.sprite = battleCard.icon;
            gradeIndicator.color = battleCard.colorIndicator;
          
          
        }
    }

    public void SetGridPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
       // Debug.Log("Clicked: " + battleCard.cardName);
       ManagerContainer.Instance.customScreenManager.HandleCardClick(this);
      
    }
}
