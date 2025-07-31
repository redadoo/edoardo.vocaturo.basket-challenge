using System.Collections;
using UnityEngine;
using Utility;
using TMPro;

namespace UIScript
{
    /// <summary>
    /// Manages the overall gameplay UI, including score display, countdown, results, and menu interactions.
    /// </summary>
    public class UIGameplay : MonoBehaviour
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
        /// Handles the end of the game, updating UI and showing results.
        /// </summary>
        private void HandleGameEnd()
        {
            ToggleGameplayUI(false);

            resultPage.SetActive(true);
            var score = UIScore.Instance.GetScore();

            playerResultText.text = score.Item1;
            enemyResultText.text = score.Item2;

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