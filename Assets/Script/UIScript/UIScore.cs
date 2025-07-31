using System;
using TMPro;
using UnityEngine;
using Utility;

namespace UIScript
{
    public class UIScore : GenericSingleton<UIScore>
    {
        [SerializeField] private TMP_Text playerScoreText;
        [SerializeField] private TMP_Text enemyScoreText;
        [SerializeField] private TMP_Text moneyPrize;

        private void Start()
        {
            SetMoneyPrize();
        }

        private void SetMoneyPrize()
        {
            int prize = GameManager.Instance.GetCurrentCampType().matchRewardValue;
            moneyPrize.text = prize.ToString();
        }

        /// <summary>
        /// Updates the score UI for either the player or the enemy.
        /// </summary>
        public void UpdateScore(bool isPlayer, int score)
        {
            if (isPlayer)
                playerScoreText.text = score.ToString();
            else
                enemyScoreText.text = score.ToString();
        }

        public Tuple<string, string> GetScore()
        {
            return Tuple.Create(playerScoreText.text, enemyScoreText.text);
        }
    }

}