using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine;
using Utility;

public class UIGameTimer : GenericSingleton<UIGameTimer>
{
    [Header("Image reference")]
    [SerializeField] private Image playerTimer;
    [SerializeField] private Image enemyTimer;

    [Header("timer")]
    [SerializeField] private float timer = 0f;
    [SerializeField] private float duration = 60f;
    [SerializeField] private float alertTime = 10f;
    [SerializeField] private bool isFlashing = false;
    [SerializeField] private float flashInterval = 0.5f;
    [SerializeField] private Color alertColor = Color.red;
    [SerializeField] private Color startColor = Color.green;

    public event Action OnGameEnd;

    void Start()
    {
        duration = GameManager.Instance.GetCurrentCampType().matchDuration;

        playerTimer.color = startColor;
        enemyTimer.color = startColor;
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float timeLeft = duration - timer;

            float fillAmount = Mathf.Clamp01(1f - (timer / duration));
            playerTimer.fillAmount = fillAmount;
            enemyTimer.fillAmount = fillAmount;

            if (!isFlashing && timeLeft <= alertTime)
            {
                StartCoroutine(FlashColor());
                isFlashing = true;
            }
        }
        else
        {
            OnGameEnd?.Invoke();
        }
    }

    IEnumerator FlashColor()
    {
        bool useAlertColor = true;

        while (true)
        {
            Color nextColor = useAlertColor ? alertColor : startColor;

            playerTimer.color = nextColor;
            enemyTimer.color = nextColor;

            useAlertColor = !useAlertColor;

            yield return new WaitForSeconds(flashInterval);
        }
    }
}
