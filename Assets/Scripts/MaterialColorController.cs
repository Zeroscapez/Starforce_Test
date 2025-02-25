using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class MaterialColorController : MonoBehaviour
{
    [Header("Color Settings")]
    [Tooltip("Shader property name for color modification")]
    public string colorProperty = "_Color";

    [Header("Transition Settings")]
    public float colorChangeSpeed = 2f;
    public bool useSmoothTransition = true;

    private Renderer[] targetRenderers;
    private Color[] originalColors;
    private MaterialPropertyBlock[] propBlocks;

    void Awake()
    {
        InitializeRenderers();
    }

    void InitializeRenderers()
    {
        // Get all renderers on this object and its children
        targetRenderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[targetRenderers.Length];
        propBlocks = new MaterialPropertyBlock[targetRenderers.Length];

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] == null) continue;

            // Initialize property blocks and store original colors
            propBlocks[i] = new MaterialPropertyBlock();
            targetRenderers[i].GetPropertyBlock(propBlocks[i]);

            if (targetRenderers[i].sharedMaterial != null)
            {
                originalColors[i] = targetRenderers[i].sharedMaterial.GetColor(colorProperty);
            }
        }
    }

    public void ChangeColor(Color newColor, bool permanent = false)
    {
        if (useSmoothTransition)
        {
            StartCoroutine(SmoothColorTransition(newColor, permanent));
        }
        else
        {
            ApplyImmediateColor(newColor, permanent);
        }
    }

    public void FlashColor(Color flashColor, float duration)
    {
        StartCoroutine(FlashColorRoutine(flashColor, duration));
    }

    public void ResetColors()
    {
        if (useSmoothTransition)
        {
            StartCoroutine(SmoothColorTransition(originalColors[0], true));
        }
        else
        {
            ApplyImmediateColor(originalColors[0], true);
        }
    }

    private IEnumerator SmoothColorTransition(Color targetColor, bool saveAsOriginal)
    {
        float t = 0f;
        Color[] startColors = new Color[targetRenderers.Length];

        // Initialize start colors
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] == null) continue;
            startColors[i] = propBlocks[i].GetColor(colorProperty);
        }

        while (t < 1f)
        {
            t += Time.deltaTime * colorChangeSpeed;

            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] == null) continue;

                Color currentColor = Color.Lerp(startColors[i], targetColor, t);
                propBlocks[i].SetColor(colorProperty, currentColor);
                targetRenderers[i].SetPropertyBlock(propBlocks[i]);
            }

            yield return null;
        }

        if (saveAsOriginal)
        {
            for (int i = 0; i < originalColors.Length; i++)
            {
                originalColors[i] = targetColor;
            }
        }
    }

    private IEnumerator FlashColorRoutine(Color flashColor, float duration)
    {
        Color[] originalTempColors = new Color[targetRenderers.Length];

        // Store current colors
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] == null) continue;
            originalTempColors[i] = propBlocks[i].GetColor(colorProperty);
        }

        // Apply flash color
        ApplyImmediateColor(flashColor, false);

        yield return new WaitForSeconds(duration);

        // Restore original colors
        if (useSmoothTransition)
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] == null) continue;
                StartCoroutine(SmoothColorTransition(originalTempColors[i], false));
            }
        }
        else
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] == null) continue;
                ApplyImmediateColor(originalTempColors[i], false);
            }
        }
    }

    private void ApplyImmediateColor(Color newColor, bool saveAsOriginal)
    {
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] == null) continue;

            propBlocks[i].SetColor(colorProperty, newColor);
            targetRenderers[i].SetPropertyBlock(propBlocks[i]);

            if (saveAsOriginal)
            {
                originalColors[i] = newColor;
            }
        }
    }

    // Public method to add new renderers at runtime
    public void AddRenderer(Renderer newRenderer)
    {
        // Expand arrays
        System.Array.Resize(ref targetRenderers, targetRenderers.Length + 1);
        System.Array.Resize(ref originalColors, originalColors.Length + 1);
        System.Array.Resize(ref propBlocks, propBlocks.Length + 1);

        int index = targetRenderers.Length - 1;
        targetRenderers[index] = newRenderer;
        propBlocks[index] = new MaterialPropertyBlock();
        newRenderer.GetPropertyBlock(propBlocks[index]);
        originalColors[index] = newRenderer.sharedMaterial.GetColor(colorProperty);
    }
}