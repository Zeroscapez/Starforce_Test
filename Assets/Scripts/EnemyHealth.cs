// EnemyHealth.cs
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 50f;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private float currentHP = 100f;
    public Material flashMaterial; // White material for flashing
    public float flashDuration = 0.1f;

    private Material originalMaterial;
    private Renderer rend;
    private float currentHealth;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
        currentHealth = maxHealth;
        GridManager.Instance.RegisterEnemy(this);

        UpdateHPText();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashEffect()); // Flash on hit
        currentHP -= damage;
        UpdateHPText();
        if (currentHealth <= 0)
        {
            Debug.Log("Die");
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
        GridManager.Instance.UnregisterEnemy(this);
        Destroy(gameObject);
    }

  

    void OnDestroy()
    {
        GridManager.Instance.UnregisterEnemy(this);
    }

    void UpdateHPText()
    {
        hpText.text = currentHP.ToString("0"); // Show HP as integer
    }
}