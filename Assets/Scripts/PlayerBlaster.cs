using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlaster : MonoBehaviour
{
    [Header("Settings")]
    public float baseDamage = 1f;         // Damage when spammed
    public float maxChargeDamage = 10f;   // Max damage at full charge
    public float chargeTime = 10f;        // Time to reach full charge
    public Color chargedColor = Color.green;

    private float currentCharge = 0f;
    private bool isCharging = true;
    private MaterialColorController colorController;

    private void Start()
    {
        // Get the material color controller
        colorController = GetComponent<MaterialColorController>();
    }

    void Update()
    {
        if (isCharging)
        {
            // Increase charge until reaching max charge time
            currentCharge = Mathf.Min(currentCharge + Time.deltaTime, chargeTime);

            // Update color when fully charged
            if (currentCharge >= chargeTime)
            {
                colorController.ChangeColor(chargedColor);
            }
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
        float damage = currentCharge >= chargeTime ? maxChargeDamage : baseDamage;

        // Get the player's current column
        int playerColumn = GridManager.Instance.GetColumnIndex(transform.position);

        // Damage logic
        for (int i = GridManager.Instance.enemies.Count - 1; i >= 0; i--)
        {
            EnemyHealth enemy = GridManager.Instance.enemies[i];
            int enemyColumn = GridManager.Instance.GetColumnIndex(enemy.transform.position);
            if (enemyColumn == playerColumn)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Reset system
        currentCharge = 0f;
        isCharging = true;
        colorController.ResetColors(); // Return to original color
    }

    public float GetChargeProgress()
    {
        return currentCharge / chargeTime;
    }
}