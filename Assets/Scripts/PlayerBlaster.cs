// PlayerBlaster.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlaster : MonoBehaviour
{
    [Header("Settings")]
    public float baseDamage = 1f;
    public float maxChargeDamage = 10f;
    public float chargeRate = 15f; // Damage per second charged
    public float spamPenalty = 5f; // Damage lost if fired too quickly

    private float currentCharge = 0f;
    private bool isCharging = true;

    void Update()
    {
        if (isCharging)
        {
            // Increase charge over time, clamped to max damage
            currentCharge = Mathf.Clamp(currentCharge + chargeRate * Time.deltaTime, 0, maxChargeDamage);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Fire();
        }
    }

    void Fire()
    {
        // Calculate final damage
        float finalDamage = maxChargeDamage;

        // Apply spam penalty (reset charge if fired too quickly)
        if (currentCharge < maxChargeDamage)
        {
            finalDamage -= spamPenalty;
            finalDamage = Mathf.Max(finalDamage, baseDamage); // Never go below base
        }

        // Get the player's current column
        int playerColumn = GridManager.Instance.GetColumnIndex(transform.position);

        // Iterate backward to safely handle modifications
        for (int i = GridManager.Instance.enemies.Count - 1; i >= 0; i--)
        {
            EnemyHealth enemy = GridManager.Instance.enemies[i];
            int enemyColumn = GridManager.Instance.GetColumnIndex(enemy.transform.position);
            if (enemyColumn == playerColumn)
            {
                enemy.TakeDamage(finalDamage);
            }
        }

        // Reset charge
        currentCharge = 0f;
        isCharging = true;
    }

    // Optional: Add a cooldown system here if needed
}