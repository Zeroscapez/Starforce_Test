using System.Collections.Generic;
using UnityEngine;

public class DeckStorage : MonoBehaviour
{
    [SerializeField] private Actor player;
    [Header("Card Data")]
    public List<BattleCard> ownedCards; // List of BattleCard assets to display.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Actor GetOwner()
    {
        return player;
    }
}
