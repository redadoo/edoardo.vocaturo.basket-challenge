using System.Collections;
using UnityEngine;
using TMPro;

public class UIMatchInfo : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField] private TMP_Text playerFee;
    [SerializeField] private TMP_Text enemyFee;
    [SerializeField] private TMP_Text totalReward;

    [Header("Animation Settings")]
    [SerializeField] private float transferDuration = 2f;

    private const int initialFeeValue = 250;
    private const int initialRewardValue = 0;

    private Coroutine transferCoroutine;

    private void OnEnable()
    {
        SetTextBoxToInitialValue();
        transferCoroutine = StartCoroutine(TransferCurrency());
    }

    private void OnDisable()
    {
        if (transferCoroutine != null)
            StopCoroutine(transferCoroutine);

        ResetValues();
    }

    private IEnumerator TransferCurrency()
    {
        int player = initialFeeValue;
        int enemy = initialFeeValue;
        int total = initialRewardValue;
        int totalToTransfer = player + enemy;
        float stepDuration = transferDuration / totalToTransfer;

        while (player > 0 || enemy > 0)
        {
            if (player > 0)
            {
                player--;
                playerFee.text = player.ToString();
            }

            if (enemy > 0)
            {
                enemy--;
                enemyFee.text = enemy.ToString();
            }

            total += 2;
            totalReward.text = total.ToString();
            yield return new WaitForSeconds(stepDuration);
        }

        GameManager.Instance.DecreaseMoney(250);
        LoadingSceneManager.Instance.LoadScene(Scene.Gameplay);
    }

    private void ResetValues()
    {
        playerFee.text = initialFeeValue.ToString();
        enemyFee.text = initialFeeValue.ToString();
        totalReward.text = initialRewardValue.ToString();
    }

    private void SetTextBoxToInitialValue()
    {
        ResetValues();
    }
}
