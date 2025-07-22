using System.Collections;
using UnityEngine;
using System;
using TMPro;

public enum GameState
{
    Playing,
    NotPlaying
}


public class GameManager : GenericSingleton<GameManager>
{
    [Header("UI GameObject")]
    [SerializeField] private GameObject shotSlider;
    [SerializeField] private GameObject scoreGameObject;
    [SerializeField] private GameObject playerUIGameObject;
    [SerializeField] private GameObject enemyUIGameObject;

    [Header("Scripts")]
    [SerializeField] private PlayerShootingSystem playerShootingSystem;

    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownBackgroundImage;

    [Header("Timer")]
    [SerializeField] private bool timerStarted = false;
    [SerializeField] private float timer = 0f;
    [SerializeField] private float timerDuration = 1f;

    public GameState gameState { get; private set; }

    public event EventHandler OnGameStart;
    public event EventHandler OnTimerEnd;

    private void Start()
    {
        InputManager.Instance.inputActions.Player.OnClick.started += OnClick_started;
        InputManager.Instance.inputActions.Player.OnClick.canceled += OnClick_canceled;
        SetUiState(false);
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        HandleTimer();
    }

    private void OnClick_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (gameState == GameState.Playing)
        {
            gameState = GameState.NotPlaying;
            timer = timerDuration;
        }
    }
    private void OnClick_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (gameState == GameState.Playing)
        {
            timerStarted = true;
        }
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        countdownBackgroundImage.SetActive(false);

        OnGameStart?.Invoke(this, EventArgs.Empty);
        
        SetUiState(true);
        gameState = GameState.Playing;
    }

    private void HandleTimer()
    {
        if (!timerStarted)
            return;

        timer += Time.deltaTime;

        if (timer >= timerDuration)
        {
            OnTimerEnd?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ResetState()
    {
        timer = 0;
    }

    private void SetUiState(bool isEnable)
    {
        shotSlider.SetActive(isEnable);
        scoreGameObject.SetActive(isEnable);
        playerUIGameObject.SetActive(isEnable);
        enemyUIGameObject.SetActive(isEnable);
    }
}
