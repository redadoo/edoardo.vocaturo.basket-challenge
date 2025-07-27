using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class UIGameplay : GenericSingleton<UIGameplay>
{
    [Header("Reference")]
    [SerializeField] private ShootingManager shootingManager;

    [Header("UI GameObject")]
    [SerializeField] private GameObject shotSlider;
    [SerializeField] private GameObject scoreGameObject;
    [SerializeField] private GameObject playerUIGameObject;
    [SerializeField] private GameObject enemyUIGameObject;
    [SerializeField] private GameObject menuUIGameObject;
    [SerializeField] private GameObject menuPageUIGameObject;
    [SerializeField] private GameObject leavePageUIGameObject;
    [SerializeField] private GameObject playerBoxes;
    [SerializeField] private GameObject resultPage;

    [SerializeField] private TMP_Text playerScore;
    [SerializeField] private TMP_Text enemyScore;

    [SerializeField] private TMP_Text playerResultScore;
    [SerializeField] private TMP_Text enemyResultScore;

    [SerializeField] private TMP_Text finalSentence;

    [Header("Cooldown")]
    [SerializeField] private GameObject countdownGameObject;
    [SerializeField] private TextMeshProUGUI countdownText;

    public event Action OnCooldownEnd;

    private void OnEnable()
    {
        if (UIGameTimer.Instance != null)
            UIGameTimer.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd()
    {
        SetUiState(false);

        resultPage.SetActive(true);
        playerResultScore.text = playerScore.text;
        enemyResultScore.text = enemyScore.text;

        if (shootingManager.IsPlayerWinner())
        {
            finalSentence.text = "You won";
            GameManager.Instance.IncreaseMoney(500);
        }
        else
        {
            finalSentence.text = "You lose";
        }

        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.5f);
        LoadingSceneManager.Instance.LoadScene(Scene.MainMenu);
    }


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
