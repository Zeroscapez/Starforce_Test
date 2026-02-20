using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomScreenManager : MonoBehaviour
{
    [Header("UI Setup")]
    public RectTransform gridParent;    // The parent panel (e.g., your CardGrid) on the Custom Screen.
    public GameObject cardPrefab;       // Your CardUI prefab.


    [Header("Card Data")]
    public List<BattleCard> availableCards; // List of BattleCard assets to display.
    [SerializeField] private DeckStorage playerDeckStorage;

    // Lists to keep track of instantiated cards and selected cards.
    private List<CardUI> cardUIs = new List<CardUI>();
    [SerializeField] private List<CardUI> selectedCards = new List<CardUI>();

    [SerializeField] private List<CardUI> chosenCards = new List<CardUI>();

    private bool selectionFinalized = false;
    private bool combatPaused = false;
    [SerializeField] private DeckViewControl deckViewControl;
    private enum SelectionDirection
    {
        None,
        Row,
        Column
    }

    private SelectionDirection currentDirection = SelectionDirection.None;


    private void Start()
    {

        availableCards = playerDeckStorage.ownedCards;
       
        BattleManager.OnBattleStart += buildCardGrid;
    

    }

    void Update()
    {



    }

    public List<CardUI> GetChosenCards()
    {
        return chosenCards;
    }

    public void buildCardGrid()
    {
        PauseCombat();
        BattleManager.Instance.SetBattleState(BattleState.CardSelect);
        gridParent.gameObject.SetActive(true);
        this.transform.parent.gameObject.SetActive(true);
        SetupGrid();
        selectionFinalized = false;
    }

    void PauseCombat()
    {
        if (!combatPaused)
        {
            Time.timeScale = 0f;
            combatPaused = true;
            // Debug.Log("Combat Paused");
        }
        else
        {
            Time.timeScale = 1f;
            combatPaused = false;
        }
    }


    /// <summary>
    /// Destroys any existing card instances.
    /// </summary>
    void ClearGrid()
    {
        foreach (var card in cardUIs)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        cardUIs.Clear();
        selectedCards.Clear();
        currentDirection = SelectionDirection.None;
    }

    /// <summary>
    /// Sets up the card grid in a 3x2 layout.
    /// </summary>
    void SetupGrid()
    {
        ClearGrid();
        int rows = 2;
        int columns = 3;
        Vector2 cellSize = new Vector2(400, 320);  // Adjust as needed.
        float spacing = 20f;

        // Calculate the total grid dimensions.
        float gridWidth = columns * cellSize.x + (columns - 1) * spacing;
        float gridHeight = rows * cellSize.y + (rows - 1) * spacing;

        // Center the grid within the gridParent (assuming its pivot is centered).
        Vector2 startPosition = new Vector2(-gridWidth / 2 + cellSize.x / 2, gridHeight / 2 - cellSize.y / 2);

        int cardIndex = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (cardIndex >= availableCards.Count)
                    return;
                
                GameObject cardGO = Instantiate(cardPrefab, gridParent);
             
                CardUI cardUI = cardGO.GetComponent<CardUI>();
                cardUI.battleCard = availableCards[cardIndex];
                cardUI.SetGridPosition(row, col);
                Vector2 cellPos = startPosition + new Vector2(col * (cellSize.x + spacing), -row * (cellSize.y + spacing));
                // Optional: add a small random offset if desired.
           
                RectTransform cardRect = cardGO.GetComponent<RectTransform>();

                // Get actual card size
                float cardWidth = cardRect.rect.width;
                float cardHeight = cardRect.rect.height;

                // Calculate allowed movement inside cell
                float maxXOffset = (cellSize.x - cardWidth) / 2f;
                float maxYOffset = (cellSize.y - cardHeight) / 2f;

                // Random offset clamped to cell bounds
                Vector2 randomOffset = new Vector2(
                    Random.Range(-maxXOffset, maxXOffset),
                    Random.Range(-maxYOffset, maxYOffset)
                );

                cardRect.anchoredPosition = cellPos + randomOffset;

                cardUIs.Add(cardUI);
                cardIndex++;
            }
        }
    }


    /// <summary>
    /// Handles selection of cards when clicked.
    /// </summary>
    public void HandleCardClick(CardUI clickedCard)
    {
        CardUI firstCard;

        // White cards bypass normal selection restrictions.
        if (clickedCard.battleCard.isWhiteCard)
        {
            ToggleSelection(clickedCard);
            return;
        }

        // If no cards are currently selected, allow any selection.
        if (selectedCards.Count == 0 || (selectedCards[0].battleCard.isWhiteCard && selectedCards.Count == 1))
        {
            ToggleSelection(clickedCard);
            return;
        }


        if (selectedCards[0].battleCard.isWhiteCard)
        {
            firstCard = selectedCards[1];
        }
        else
        {
            firstCard = selectedCards[0];
        }
     

        // Check if the card shares the same name.
        bool sameName = (firstCard.battleCard.cardName == clickedCard.battleCard.cardName);

        // Check if the cards are in the same column.
        bool sameColumn = firstCard.Column == clickedCard.Column;

        bool sameRow = firstCard.Row == clickedCard.Row;


        if (selectedCards.Count == 1)
        {
            if (sameName)
            {
                ToggleSelection(clickedCard);
                return;
            }

            if (sameColumn)
            {
                currentDirection = SelectionDirection.Column;
                ToggleSelection(clickedCard);
                return;
            }

            if (sameRow)
            {
                currentDirection = SelectionDirection.Row;
                ToggleSelection(clickedCard);
                return;
            }

            Debug.Log("Second selection must match row, column, or name.");
            return;
        }

        // THIRD+ CARD must respect locked direction
        if (sameName)
        {
            ToggleSelection(clickedCard);
            return;
        }

        if (currentDirection == SelectionDirection.Column && sameColumn)
        {
            ToggleSelection(clickedCard);
            return;
        }

        if (currentDirection == SelectionDirection.Row && sameRow)
        {
            ToggleSelection(clickedCard);
            return;
        }

        Debug.Log("Selection locked to " + currentDirection);
    }

    //Toggles the selection of a selected card
    //I.E. if a card is selected it will be unselected and if it is unselected it will be selected, also handles visual feedback for selection
    void ToggleSelection(CardUI cardUI)
    {
        if (selectedCards.Contains(cardUI))
        {
            
            selectedCards.Remove(cardUI);

            if (selectedCards.Count <= 1)
            {
                currentDirection = SelectionDirection.None;
            }

            // Reset visual feedback 
            cardUI.visualElement.SetActive(true);
        }
        else
        {
           
            selectedCards.Add(cardUI);
            // Provide visual feedback to show the card is selected
           cardUI.visualElement.SetActive(false);
        }
    }

    public void FinalizeSelection()
    {
        chosenCards.Clear();
        //Debug.Log("Finalizing selection. Selected Cards:");
        foreach (var card in selectedCards)
        {
            Debug.Log(card.battleCard.cardName);
            chosenCards.Add(card);
        }
        AudioManager.Instance.PlaySFX("CustomConfirm");
        selectionFinalized = true;
        selectedCards.Clear();
        deckViewControl.UpdateDeckView();
        BattleManager.Instance.SetBattleState(BattleState.PlayerTurn);

        PauseCombat(); // Unpause combat when selection is finalized
        this.transform.parent.gameObject.SetActive(false);
        
   
    }

   
}
