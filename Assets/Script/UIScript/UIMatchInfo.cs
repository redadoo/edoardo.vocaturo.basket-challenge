using System.Collections;
using UnityEngine;
using AudioSystem;
using TMPro;

public class UIMatchInfo : MonoBehaviour
{
    [Header("Match Settings")]
    [SerializeField] private TMP_Text playerFee;
    [SerializeField] private TMP_Text enemyFee;
    [SerializeField] private TMP_Text totalReward;
    [SerializeField] private CampTypeSO campTypeSO;
    [SerializeField] private int initialFeeValue;
    [SerializeField] private int initialRewardValue;

    [Header("Animation Settings")]
    [SerializeField] private float transferDuration;

    [Header("SoundData")]
    [SerializeField] private SoundData moneySoundData;


    private Coroutine transferCoroutine;

    private void OnEnable()
    {
        SetTextBoxToInitialValue();

        transferCoroutine = StartCoroutine(TransferCurrency());

        SoundManager.Instance.CreateSound()
            .WithSoundData(moneySoundData)
            .Play();
    }

    private void OnDisable()
    {
        if (transferCoroutine != null)
            StopCoroutine(transferCoroutine);
    }

    private IEnumerator TransferCurrency()
    {
        int enemy = initialFeeValue;
        int player = initialFeeValue;
        int total = 0;
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

        GameManager.Instance.ChangeMoney(-campTypeSO.matchAdmissionFee);
        LoadingSceneManager.Instance.LoadScene(Scene.Gameplay);
    }

    private void SetTextBoxToInitialValue()
    {
        transferDuration = 1;

        campTypeSO = GameManager.Instance.GetCurrentCampType();
        initialFeeValue = campTypeSO.matchAdmissionFee;
        initialRewardValue = campTypeSO.matchRewardValue;

        playerFee.text = initialFeeValue.ToString();
        enemyFee.text = initialFeeValue.ToString();

        totalReward.text = "";
    }
}
