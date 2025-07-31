using UnityEngine;
using TMPro;

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
