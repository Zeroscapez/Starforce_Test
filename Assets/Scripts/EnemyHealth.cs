// EnemyHealth.cs
using System;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 50;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private float currentHP = 100f;
    public Material flashMaterial; // White material for flashing
    public float flashDuration = 0.1f;
   
    private Material originalMaterial;
    private Renderer rend;
    private int currentHealth;


    public static event Action<EnemyAI> Death;


    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
        currentHealth = maxHealth;
       

        UpdateHPText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashEffect()); // Flash on hit
        currentHP -= damage;
        UpdateHPText();
        if (currentHealth <= 0)
        {
         
            Die();
        }
    }

    System.Collections.IEnumerator FlashEffect()
    {
        rend.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        rend.material = originalMaterial;
    }

    void Die()
    {
        Death?.Invoke(GetComponent<EnemyAI>()); // Notify BattleManager of death
        Destroy(gameObject);
    }

  

    void OnDestroy()
    {
     
    }

    void UpdateHPText()
    {
        hpText.text = currentHP.ToString("0"); // Show HP as integer
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}