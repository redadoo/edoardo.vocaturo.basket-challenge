using UnityEngine;
using TMPro;

namespace UIScript
{
    /// <summary>
    /// Handles the display of the match result UI, showing player and enemy scores,
    /// and indicating the winner visually.
    /// </summary>
    public class UIMatchResult : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerPointsText;
        [SerializeField] private TMP_Text enemyPointsText;

        [SerializeField] private GameObject playerWinnerGameObject;
        [SerializeField] private GameObject enemyWinnerGameObject;

        private void Start()
        {
            SetPlayersScore();
        }

        /// <summary>
        /// Updates the score UI elements with the last match points from GameManager
        /// and activates the winner GameObject accordingly.
        /// </summary>
        public void SetPlayersScore()
        {
            int playerPoints = GameManager.Instance.playerLastMatchPoints;
            int enemyPoints = GameManager.Instance.enemyLastMatchPoints;

            playerPointsText.text = playerPoints.ToString();
            enemyPointsText.text = enemyPoints.ToString();

            if (playerPoints > enemyPoints)
                playerWinnerGameObject.SetActive(true);
            else
                enemyWinnerGameObject.SetActive(true);
        }
    }
}