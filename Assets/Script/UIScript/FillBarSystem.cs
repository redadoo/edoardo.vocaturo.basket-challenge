using UnityEngine.UI;
using UnityEngine;

public class FillBarSystem : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform fillArea;
    [SerializeField] private RectTransform indicator;
    [SerializeField] private RectTransform perfectShotImage;
    [SerializeField] private RectTransform highShotImage;

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
            return;

        UpdateFillAmountFromInput();
        UpdateIndicatorPosition();
    }

    /// <summary>
    /// Updates the vertical position of the indicator based on fill amount.
    /// </summary>
    private void UpdateIndicatorPosition()
    {
        float yPosition = GetYFromFillAmount(fillImage.fillAmount);
        indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, yPosition);
    }

    /// <summary>
    /// Converts fill amount (0 to 1) into Y offset inside fill area.
    /// </summary>
    private float GetYFromFillAmount(float fillAmount)
    {
        float totalHeight = fillArea.rect.height;
        return (fillAmount - 0.5f) * totalHeight;
    }

    /// <summary>
    /// Displays shot range indicators at the appropriate fill levels.
    /// </summary>
    public void SetShotRange(float perfectShotStart, float highShotStart)
    {
        if (perfectShotImage != null)
        {
            perfectShotImage.gameObject.SetActive(true);
            perfectShotImage.anchoredPosition = new Vector2(
                perfectShotImage.anchoredPosition.x,
                GetYFromFillAmount(perfectShotStart)
            );
        }

        if (highShotImage != null)
        {
            highShotImage.gameObject.SetActive(true);
            highShotImage.anchoredPosition = new Vector2(
                highShotImage.anchoredPosition.x,
                GetYFromFillAmount(highShotStart)
            );
        }
    }

    /// <summary>
    /// Updates the fill amount based on the drag distance from the initial touch.
    /// </summary>
    private void UpdateFillAmountFromInput()
    {
        Vector2? firstTouch = InputManager.Instance.firstTouchPos;
        Vector2 currentTouch = InputManager.Instance.touchPos;

        if (firstTouch.HasValue)
        {
            float distance = Vector2.Distance(firstTouch.Value, currentTouch);
            float normalized = Mathf.Clamp01(distance / 2400f);

            if (normalized > fillImage.fillAmount)
            {
                fillImage.fillAmount = normalized;
            }
        }
    }

    public float GetFillAmount() =>
        fillImage.fillAmount;
}
