using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGameTimer : MonoBehaviour
{
    [Header("Image reference")]
    [SerializeField] private Image playerTimer;
    [SerializeField] private Image enemyTimer;

    [Header("timer")]
    [SerializeField] private float timer = 0f;
    [SerializeField] private float duration = 60f;
    [SerializeField] private float alertTime = 10f;
    [SerializeField] private float flashInterval = 0.5f;
    [SerializeField] private bool isFlashing = false;
    [SerializeField] private Color alertColor = Color.red;
    [SerializeField] private Color startColor = Color.green;

    void Start()
    {
        playerTimer.fillAmount = 1f;
        enemyTimer.fillAmount = 1f;

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
