using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewControl : MonoBehaviour
{
    public BattleCardManager cardManager;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDamage;
    public GameObject curCardImage;
    public Image cardImage;
    public GameObject cardGradeBox;
    public Transform cardGradeTracker;

    // Start is called before the first frame update
    void Start()
    {
        cardManager = FindObjectOfType<BattleCardManager>();
        cardName.text = "";
        cardDamage.text = "";
        cardImage.sprite = null;
        curCardImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateDeckView()
    {
        // Check if selectedCards is not null and contains at least one card
        if (cardManager.selectedCards != null && cardManager.selectedCards.Count > 0)
        {
            // Clear previous cardGradeBox instances
            foreach (Transform child in cardGradeTracker)
            {
                Destroy(child.gameObject);
            }
            curCardImage.SetActive(true);
            // Update card details for the first card
            cardName.text = cardManager.selectedCards[0].battleCard.cardName;
            cardImage.sprite = cardManager.selectedCards[0].battleCard.icon;
            cardDamage.text = cardManager.selectedCards[0].battleCard.damage.ToString();

            // Instantiate new cardGradeBox objects for the remaining cards, starting from the back
            for (int i = cardManager.selectedCards.Count - 1; i > 0; i--)
            {
                var newCard = Instantiate(cardGradeBox, cardGradeTracker);
                newCard.GetComponent<Image>().color = cardManager.selectedCards[i].battleCard.colorIndicator;
            }
        }
        else
        {
            // If no cards are selected or the first card is null, handle the fallback
            if (curCardImage != null)
            {
                curCardImage.SetActive(false);
            }

            cardName.text = "";
            cardDamage.text = "";
            cardImage.sprite = null;
        }
    }



}
