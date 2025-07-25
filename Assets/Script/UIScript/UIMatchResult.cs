using UnityEngine;
using TMPro;

public class UIMatchResult : MonoBehaviour
{
    [SerializeField] private TMP_Text playerPointsText;
    [SerializeField] private TMP_Text enemyPointsText;

    [SerializeField] private GameObject playerWinnerGameObject;
    [SerializeField] private GameObject enemyWinnerGameObject;


    private void OnGameFinish()
    {
        int playerRes = 24;
        int enemyRes = 21;

        playerPointsText.text = playerRes.ToString();
        enemyPointsText.text = enemyRes.ToString();

        if (playerRes > enemyRes)
            playerWinnerGameObject.SetActive(true);
        else
            enemyWinnerGameObject.SetActive(true);
    }
}
