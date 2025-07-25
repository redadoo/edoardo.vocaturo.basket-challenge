using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class UIGameplay : GenericSingleton<UIGameplay>
{
    [Header("UI GameObject")]
    [SerializeField] private GameObject shotSlider;
    [SerializeField] private GameObject scoreGameObject;
    [SerializeField] private GameObject playerUIGameObject;
    [SerializeField] private GameObject enemyUIGameObject;
    [SerializeField] private GameObject menuUIGameObject;
    [SerializeField] private GameObject menuPageUIGameObject;
    [SerializeField] private GameObject leavePageUIGameObject;
    [SerializeField] private GameObject playerBoxes;

    [SerializeField] private TMP_Text playerScore;
    [SerializeField] private TMP_Text enemyScore;

    [Header("Cooldown")]
    [SerializeField] private GameObject countdownGameObject;
    [SerializeField] private TextMeshProUGUI countdownText;

    public event Action OnCooldownEnd;

    private void Start()
    {
        SetUiState(false);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownGameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownGameObject.SetActive(false);
        SetUiState(true);

        OnCooldownEnd?.Invoke();
    }

    private void SetUiState(bool isEnable)
    {
        playerUIGameObject.SetActive(isEnable);
        enemyUIGameObject.SetActive(isEnable);
        menuUIGameObject.SetActive(isEnable);
        scoreGameObject.SetActive(isEnable);
        playerBoxes.SetActive(isEnable);
        shotSlider.SetActive(isEnable);
    }

    public void IncreaseScore(bool isPlayer, int points)
    {
        if (isPlayer)
            playerScore.text = points.ToString();
        else
            enemyScore.text = points.ToString();
    }
}
