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
    public void SetShotRange(ShotInfoSO shotInfo)
    {

        if (highShotImage != null)
        {
            highShotImage.gameObject.SetActive(true);
            highShotImage.anchoredPosition = new Vector2(
                highShotImage.anchoredPosition.x,
                GetYFromFillAmount(shotInfo.highMin)
            );
        }

        if (perfectShotImage != null)
        {
            perfectShotImage.gameObject.SetActive(true);
            perfectShotImage.anchoredPosition = new Vector2(
                perfectShotImage.anchoredPosition.x,
                GetYFromFillAmount(shotInfo.perfectMin)
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
                fillImage.fillAmount = normalized;
        }
    }

    public void ResetValue()
    {
        fillImage.fillAmount = 0;
    }

    public float GetFillAmount() =>
        fillImage.fillAmount;
}
