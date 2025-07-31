using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIScript;
using TMPro;

/// <summary>
/// Controls the visual bonus indicator and emission logic for the backboard during gameplay.
/// </summary>
public class BackboardBonus : MonoBehaviour
{
    [Header("Visual Components")]
    [SerializeField] private Material backboardMaterial;
    [SerializeField] private TMP_Text bonusText;

    [Header("Bonus Settings")]
    [SerializeField] private float interval = 20f;

    private readonly Dictionary<Color, int> bonusByColor = new();
    private Color activeColor;

    public bool wasHit;

    private void Start()
    {
        backboardMaterial = GetComponent<MeshRenderer>().material;

        InitializeBonusColors();
        StartCoroutine(BonusCycleRoutine());
    }

    /// <summary>
    /// Initializes the dictionary of possible bonus colors and values.
    /// </summary>
    private void InitializeBonusColors()
    {
        bonusByColor.Clear();
        bonusByColor[Color.yellow] = 8;
        bonusByColor[Color.blue] = 6;
        bonusByColor[Color.green] = 4;
    }

    /// <summary>
    /// Coroutine that controls the timing and activation of backboard bonuses.
    /// </summary>
    private IEnumerator BonusCycleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            UIFeedback.Instance.ShowBackboardBonus();
            ActivateBonus();

            wasHit = false;
            yield return new WaitUntil(() => wasHit);

            DeactivateBonus();
        }
    }

    /// <summary>
    /// Activates the emission and sets a random bonus color.
    /// </summary>
    private void ActivateBonus()
    {
        if (backboardMaterial == null || bonusByColor.Count == 0) return;

        Color chosenColor = GetRandomBonusColor();
        int bonusValue = bonusByColor[chosenColor];

        SetEmission(chosenColor);
        SetBonusText(bonusValue, chosenColor);
    }

    /// <summary>
    /// Deactivates the emission and hides the bonus text.
    /// </summary>
    private void DeactivateBonus()
    {
        if (backboardMaterial == null) return;

        backboardMaterial.SetColor("_EmissionColor", Color.black);
        backboardMaterial.DisableKeyword("_EMISSION");

        bonusText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the emission color for the material.
    /// </summary>
    private void SetEmission(Color color)
    {
        backboardMaterial.EnableKeyword("_EMISSION");
        backboardMaterial.SetColor("_EmissionColor", color * 2f);
    }

    /// <summary>
    /// Displays the bonus value on screen with the associated color.
    /// </summary>
    private void SetBonusText(int bonusValue, Color color)
    {
        bonusText.text = $"+{bonusValue}";
        bonusText.gameObject.SetActive(true);
        activeColor = color;
    }

    /// <summary>
    /// Selects a random color from the available bonus colors.
    /// </summary>
    private Color GetRandomBonusColor()
    {
        List<Color> colors = new(bonusByColor.Keys);
        return colors[Random.Range(0, colors.Count)];
    }

    /// <summary>
    /// Returns the bonus value associated with the current active color.
    /// </summary>
    public int GetBonusValue() =>
         bonusByColor.TryGetValue(activeColor, out int value) ? value : 0;
}
