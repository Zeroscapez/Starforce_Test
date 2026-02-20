using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager Instance { get; private set; }
    public GameObject NoiseGauge;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
