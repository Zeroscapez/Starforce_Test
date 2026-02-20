using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public string actorName;
    private DeckStorage deckStorage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deckStorage = GetComponentInChildren<DeckStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
