using UnityEngine;
using System.Collections.Generic;

public class BattleCardManager : MonoBehaviour
{
    [Header("UI Setup")]
    public RectTransform gridParent;    // The parent panel (e.g., your CardGrid) on the Custom Screen.
    public GameObject cardPrefab;       // Your CardUI prefab.

    [Header("Card Data")]
    public List<BattleCard> availableCards; // List of BattleCard assets to display.

    // Lists to keep track of instantiated cards and selected cards.
    private List<CardUI> cardUIs = new List<CardUI>();
    [SerializeField]private List<CardUI> selectedCards = new List<CardUI>();

    private bool combatPaused = false;


    void Update()
    {
        // When you press the R key, clear the grid and instantiate a new 3x2 layout.
        if (Input.GetKeyDown(KeyCode.R))
        {
            PauseCombat();
            ClearGrid();
            SetupGrid();
        }

        // For testing purposes, you can use the T key to resume combat.
        if (Input.GetKeyDown(KeyCode.T))
        {
            ResumeCombat();
        }
    }

    void PauseCombat()
    {
        if(!combatPaused)
        {
            Time.timeScale = 0f;
            combatPaused = true;
            Debug.Log("Combat Paused");
        }
    }

    void ResumeCombat()
    {
        if (combatPaused)
        {
            Time.timeScale = 1f;
            combatPaused = false;
            Debug.Log("Combat resumed.");
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
    }

    /// <summary>
    /// Sets up the card grid in a 3x2 layout.
    /// </summary>
    void SetupGrid()
    {
        int rows = 2;
        int columns = 3;
        Vector2 cellSize = new Vector2(300, 220);  // Adjust as needed.
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

                Vector2 cellPos = startPosition + new Vector2(col * (cellSize.x + spacing), -row * (cellSize.y + spacing));
                // Optional: add a small random offset if desired.
                Vector2 randomOffset = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
                cardGO.GetComponent<RectTransform>().anchoredPosition = cellPos + randomOffset;

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
        // White cards bypass normal selection restrictions.
        if (clickedCard.battleCard.isWhiteCard)
        {
            ToggleSelection(clickedCard);
            return;
        }

        // If no cards are currently selected, allow any selection.
        if (selectedCards.Count == 0)
        {
            ToggleSelection(clickedCard);
            return;
        }

        // Use the first selected card as the reference.
        CardUI firstCard = selectedCards[0];

        // Check if the card shares the same name.
        bool sameName = (firstCard.battleCard.cardName == clickedCard.battleCard.cardName);

        // Check if the cards are in the same column.
        bool sameColumn = Mathf.Abs(firstCard.GetComponent<RectTransform>().anchoredPosition.x -
                                      clickedCard.GetComponent<RectTransform>().anchoredPosition.x) < 5f;

        // Check if the cards are in the same row.
        bool sameRow = Mathf.Abs(firstCard.GetComponent<RectTransform>().anchoredPosition.y -
                                   clickedCard.GetComponent<RectTransform>().anchoredPosition.y) < 5f;

        if (sameName || sameColumn || sameRow)
        {
            ToggleSelection(clickedCard);
        }
        else
        {
            Debug.Log("Selection rule violated: Cards must be in the same column/row or share the same name.");
        }
    }

    /// <summary>
    /// Toggles the selection state of a card.
    /// </summary>
    void ToggleSelection(CardUI cardUI)
    {
        if (selectedCards.Contains(cardUI))
        {
            selectedCards.Remove(cardUI);
            // Reset visual feedback (for example, revert background color)
            cardUI.background.color = cardUI.battleCard.colorIndicator;
        }
        else
        {
            selectedCards.Add(cardUI);
            // Provide visual feedback to show the card is selected (e.g., change the background color)
            cardUI.background.color = Color.green;
        }
    }
}
