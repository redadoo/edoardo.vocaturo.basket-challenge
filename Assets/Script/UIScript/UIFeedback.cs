using System.Collections;
using UnityEngine;
using Utility;
using TMPro;

/// <summary>
/// Handles UI feedback such as score popups, bonus messages, and visual effects 
/// </summary>
public class UIFeedback : GenericSingleton<UIFeedback>
{
    [Header("Popup Elements")]
    [Tooltip("Popup object displayed when the player scores.")]
    [SerializeField] private GameObject popupGameObject;

    [Header("Player UI")]
    [Tooltip("Primary text box for displaying special player messages.")]
    [SerializeField] private TMP_Text playerFirstTextBox;

    [Tooltip("Secondary text box for displaying player score.")]
    [SerializeField] private TMP_Text playerSecondTextBox;

    [Header("Enemy UI")]
    [Tooltip("Text box for displaying enemy score.")]
    [SerializeField] private TMP_Text enemyTextBox;

    [Header("Bonus UI")]
    [Tooltip("Text box shown when a backboard bonus is activated.")]
    [SerializeField] private TMP_Text backboardTextBox;

    private const int PerfectShotScore = 3;
    private const string PerfectShotMessage = "PERFECT SHOT";

    /// <summary>
    /// Displays the backboard bonus message and fades it out over time.
    /// </summary>
    public void ShowBackboardBonus()
    {
        backboardTextBox.gameObject.SetActive(true);
        StartCoroutine(FadeAndHide(backboardTextBox.gameObject));
    }

    /// <summary>
    /// Displays the score feedback based on whether it was the player or the enemy who scored.
    /// </summary>
    public void ShowScore(bool isPlayer, int points)
    {
        if (isPlayer) ShowPlayerScore(points);
        else ShowEnemyScore(points);
    }

    /// <summary>
    /// Displays the score message for the enemy.
    /// </summary>
    private void ShowEnemyScore(int points)
    {
        enemyTextBox.text = GetPointsString(points);
        enemyTextBox.color = points == PerfectShotScore ? Color.green : Color.yellow;
        enemyTextBox.gameObject.SetActive(true);

        StartCoroutine(FadeAndHide(enemyTextBox.gameObject));
    }

    /// <summary>
    /// Displays the score popup for the player.
    /// </summary>
    private void ShowPlayerScore(int points)
    {
        popupGameObject.SetActive(true);

        if (points == PerfectShotScore)
        {
            playerFirstTextBox.text = PerfectShotMessage;
            playerFirstTextBox.color = Color.green;
            playerSecondTextBox.color = Color.green;
        }
        else
        {
            playerFirstTextBox.text = "";
            playerSecondTextBox.color = Color.yellow;
        }

        playerSecondTextBox.text = GetPointsString(points);

        StartCoroutine(FadeAndHide(popupGameObject));
    }

    /// <summary>
    /// Fades out the specified GameObject over time and resets its position and visibility.
    /// </summary>
    private IEnumerator FadeAndHide(GameObject target)
    {
        CanvasGroup group = target.GetComponent<CanvasGroup>();
        if (group == null)
            group = target.AddComponent<CanvasGroup>();

        group.alpha = 1f;
        Vector3 startPosition = target.transform.position;
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            target.transform.position = startPosition + Vector3.up * 20f * (elapsed / duration);
            yield return null;
        }

        target.SetActive(false);
        target.transform.position = startPosition;
        group.alpha = 1f;
    }

    /// <summary>
    /// Converts a point value to a formatted string for UI display.
    /// </summary>
    /// <returns>A string in the format "+X pt".</returns>
    private string GetPointsString(int points) =>
        $"+{points} pt";
}
