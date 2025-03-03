using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBlaster : MonoBehaviour
{
    [Header("Settings")]
    public float baseDamage = 1f;         // Damage when spammed
    public float maxChargeDamage = 10f;   // Max damage at full charge
    public float chargeTime = 10f;        // Time to reach full charge
    public Color chargedColor = Color.green;
    public Image chargetracker;

    public int flashCount;
    public float flashDuration;

    public AudioClip blasterFireSound;
    public AudioClip blasterFullChargeSound;



    private float currentCharge = 0f;
    private bool isCharging = true;
    private MaterialColorController colorController;

    public NoiseManager noiseManager;


    private Color originalChargeTrackerColor;
    private bool hasFlashedChargetracker = false;
    private bool hasPlayedFullyChargedSound = false;

    private void Start()
    {
        // Get the material color controller
        colorController = GetComponent<MaterialColorController>();
        if (chargetracker != null)
        {
            originalChargeTrackerColor = chargetracker.color;
            chargetracker.fillAmount = 0f;
        }
    }

    void Update()
    {
        if (isCharging)
        {
            // Increase charge until reaching max charge time
            currentCharge = Mathf.Min(currentCharge + Time.deltaTime, chargeTime);
            if (chargetracker != null)
            {
                chargetracker.fillAmount = GetChargeProgress();
            }
                

            // When fully charged, change color and flash the tracker if not already done
            if (currentCharge >= chargeTime && !hasFlashedChargetracker)
            {
                colorController.ChangeColor(chargedColor);

                if (!hasPlayedFullyChargedSound && blasterFullChargeSound != null)
                {
                    AudioManager.Instance.PlaySound("BlasterFullSound");

                    hasPlayedFullyChargedSound = true;
                }


                StartCoroutine(FlashTracker());
                hasFlashedChargetracker = true;
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

        if (blasterFireSound != null)
        {
            AudioManager.Instance.PlaySound("BlasterFireSound");
        }

        // Get the player's current column
        int playerColumn = GridManager.Instance.GetColumnIndex(transform.position);
       // Debug.Log(playerColumn);
        // Loop through registered enemies (using a copy in case the list is modified)
        List<EnemyHealth> enemies = new List<EnemyHealth>(GridManager.Instance.registeredEnemies);

        // Damage logic
        foreach (EnemyHealth enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            int enemyColumn = 0;

            if (enemyAI != null)
            {
                Vector2Int enemyGridPos = enemyAI.GetGridPosition();
                enemyColumn = enemyGridPos.x;
            }
            else
            {
                enemyColumn = GridManager.Instance.GetColumnIndex(enemy.transform.position);
            }

            if(enemyColumn == playerColumn)
            {
                enemy.TakeDamage(damage);
                noiseManager.NoiseGain(damage);
            }
        }

        //Reset
        currentCharge = 0f;
        isCharging = true;
        colorController.ResetColors();
        if (chargetracker != null)
        {
            chargetracker.fillAmount = 0f;
            chargetracker.color = originalChargeTrackerColor;
        }
        hasFlashedChargetracker = false;
        hasPlayedFullyChargedSound = false;
}

    public float GetChargeProgress()
    {
        return currentCharge / chargeTime;
    }

    IEnumerator FlashTracker()
    {
        flashCount = 2;
        flashDuration = 0.1f;

        for (int i = 0; i < flashCount; i++) {
            if (chargetracker != null)
            {
                chargetracker.color = Color.white;
            }
            yield return new WaitForSecondsRealtime(flashDuration);
            if (chargetracker != null)
            {
                chargetracker.color = chargedColor;
            }
            yield return new WaitForSecondsRealtime(flashDuration);
        }
        // Ensure it ends on the charged color
        if (chargetracker != null)
            chargetracker.color = chargedColor;
    }

    }
