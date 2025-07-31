using System.Collections;
using UnityEngine;
using AudioSystem;
using TMPro;

namespace UIScript
{
    /// <summary>
    /// Manages the UI display of match fees and rewards, animating the transfer of currency
    /// between player and enemy fees into the total reward.
    /// </summary>
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


        /// <summary>
        /// Coroutine animating the gradual transfer of fees from player and enemy to total reward.
        /// Upon completion, deducts the match admission fee from the player's money and loads the gameplay scene.
        /// </summary>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator TransferCurrency()
        {
            int enemy = initialFeeValue;
            int player = initialFeeValue;
            int totalToTransfer = player + enemy;

            float elapsed = 0f;
            float duration = transferDuration;

            while (elapsed < duration && (player > 0 || enemy > 0))
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                int amountTransferred = Mathf.FloorToInt(progress * totalToTransfer);

                int newPlayer = Mathf.Max(0, initialFeeValue - (amountTransferred / 2));
                int newEnemy = Mathf.Max(0, initialFeeValue - (amountTransferred / 2));
                int newTotal = amountTransferred;

                if (newPlayer != player)
                {
                    player = newPlayer;
                    playerFee.text = player.ToString();
                }

                if (newEnemy != enemy)
                {
                    enemy = newEnemy;
                    enemyFee.text = enemy.ToString();
                }

                totalReward.text = newTotal.ToString();

                yield return null;
            }

            playerFee.text = "0";
            enemyFee.text = "0";
            totalReward.text = (initialFeeValue * 2).ToString();

            GameManager.Instance.ChangeMoney(-campTypeSO.matchAdmissionFee);
            LoadingSceneManager.Instance.LoadScene(Scene.Gameplay);
        }


        /// <summary>
        /// Initializes UI text fields to their starting values based on the current camp type.
        /// Resets the transfer duration and retrieves fee and reward values.
        /// </summary>
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

}