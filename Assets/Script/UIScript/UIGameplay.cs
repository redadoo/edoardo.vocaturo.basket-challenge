using System.Collections;
using UnityEngine;
using Utility;
using TMPro;

namespace UIScript
{
    /// <summary>
    /// Manages the overall gameplay UI, including score display, countdown, results, and menu interactions.
    /// </summary>
    public class UIGameplay : GenericSingleton<UIGameplay>
    {
        [Header("Managers")]
        [SerializeField] private ShootingManager shootingManager;

        [Header("Script")]
        [SerializeField] private UICountdown uICountdown;

        [Header("UI Elements")]
        [SerializeField] private GameObject shotSlider;
        [SerializeField] private GameObject fireballSlider;
        [SerializeField] private GameObject scoreDisplay;
        [SerializeField] private GameObject playerUI;
        [SerializeField] private GameObject enemyUI;
        [SerializeField] private GameObject menuUI;
        [SerializeField] private GameObject menuPage;
        [SerializeField] private GameObject leavePage;
        [SerializeField] private GameObject playerBoxes;
        [SerializeField] private GameObject resultPage;

        [Header("Score Texts")]
        [SerializeField] private TMP_Text playerScoreText;
        [SerializeField] private TMP_Text enemyScoreText;
        [SerializeField] private TMP_Text playerResultText;
        [SerializeField] private TMP_Text enemyResultText;
        [SerializeField] private TMP_Text finalMessage;

        private void Start()
        {
            ToggleGameplayUI(false);
        }

        private void OnEnable()
        {
            if (uICountdown != null)
                uICountdown.OnCooldownEnd += OnCooldownEnd;
            if (UIGameTimer.Instance != null)
                UIGameTimer.Instance.OnGameEnd += HandleGameEnd;
        }

        private void OnCooldownEnd()
        {
            ToggleGameplayUI(true);
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

        /// <summary>
        /// Handles the end of the game, updating UI and showing results.
        /// </summary>
        private void HandleGameEnd()
        {
            ToggleGameplayUI(false);

            resultPage.SetActive(true);
            playerResultText.text = playerScoreText.text;
            enemyResultText.text = enemyScoreText.text;

            if (shootingManager.IsPlayerWinner())
            {
                finalMessage.text = "You won";
                GameManager.Instance.GiveMoneyReward();
            }
            else
            {
                finalMessage.text = "You lose";
            }

            StartCoroutine(ReturnToMainMenuAfterDelay());
        }


        /// <summary>
        /// Waits for a short delay before returning to the main menu.
        /// </summary>
        private IEnumerator ReturnToMainMenuAfterDelay()
        {
            yield return new WaitForSeconds(3f);
            LoadingSceneManager.Instance.LoadScene(Scene.MainMenu);
        }

        /// <summary>
        /// Enables or disables the main gameplay UI elements.
        /// </summary>
        private void ToggleGameplayUI(bool isEnabled)
        {
            shotSlider.SetActive(isEnabled);
            fireballSlider.SetActive(isEnabled);
            scoreDisplay.SetActive(isEnabled);
            playerUI.SetActive(isEnabled);
            enemyUI.SetActive(isEnabled);
            menuUI.SetActive(isEnabled);
            playerBoxes.SetActive(isEnabled);
        }
    }

}