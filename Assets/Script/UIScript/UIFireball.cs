using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFireball : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fillBar;

    [Header("Animation Durations")]
    [SerializeField] private float fillDuration = 1f;
    [SerializeField] private float emptyDuration = 10f;

    [Header("Point Thresholds")]
    [SerializeField] private int perfectShotValue = 3;

    private Coroutine currentAnimation;

    private const float TwoPointFill = 1f / 6f;
    private const float ThreePointFill = 1f / 3f;
    private const float MaxFill = 1f;
    private const float MinFill = 0f;

    public event Action OnFireballStart;
    public event Action OnFireballEnd;

    private void OnDisable()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }
    }


    /// <summary>
    /// Call when a shot is successfully made.
    /// </summary>
    public void OnShotMade(int points)
    {
        float fillIncrement = (points == perfectShotValue) ? ThreePointFill : TwoPointFill;
        float newFillAmount = fillBar.fillAmount + fillIncrement;

        if (newFillAmount >= MaxFill)
        {
            TriggerFireballSequence();
        }
        else
        {
            AnimateToFill(newFillAmount, fillDuration);
        }
    }

    /// <summary>
    /// Call when a shot is missed.
    /// </summary>
    public void OnShotMissed()
    {
        AnimateToFill(MinFill, fillDuration);
    }

    private void AnimateToFill(float targetFill, float duration)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateFill(targetFill, duration));
    }

    private void TriggerFireballSequence()
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        fillBar.fillAmount = MaxFill;
        OnFireballStart?.Invoke();
        currentAnimation = StartCoroutine(AnimateFireballEmpty());
    }

    private IEnumerator AnimateFireballEmpty()
    {
        yield return AnimateFill(MinFill, emptyDuration);

        currentAnimation = null;
        OnFireballEnd?.Invoke();
    }

    private IEnumerator AnimateFill(float target, float duration)
    {
        float startFill = fillBar.fillAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fillBar.fillAmount = Mathf.Lerp(startFill, target, elapsed / duration);
            yield return null;
        }

        fillBar.fillAmount = target;
    }
}
