using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [Header("Lock-On")]
    public bool isLockedOn;
    public Transform lockOnPosition; // Assign in Inspector (empty GameObject in front of enemy)
    public GameObject lockOnIndicator; // Visual highlight (e.g., particle system)

    void Update()
    {
        // Show/hide lock-on indicator
        lockOnIndicator.SetActive(isLockedOn);
    }
}