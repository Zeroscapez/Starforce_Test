// EnemyHealth.cs
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 50f;
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
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashEffect()); // Flash on hit

        if (currentHealth <= 0) Die();
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
}