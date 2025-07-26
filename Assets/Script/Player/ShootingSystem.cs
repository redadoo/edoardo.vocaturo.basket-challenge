using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : GenericSingleton<ShootingSystem>
{
    [Header("Gameplay Settings")]
    [SerializeField] private List<ShotInfoSO> shotRanges;
    [SerializeField] private bool isPossibleToShoot = false;
    [SerializeField] private int currentPositionIndex = 0;
    [SerializeField] private int scoreCount = 0;
    [SerializeField] private bool hasScored = false;

    [SerializeField] private int playerPoints = 0;
    [SerializeField] private int enemyPoints = 0;
    [SerializeField] private ShotType lastShotType;

    [Header("References")]
    [SerializeField] private FillBarSystem fillBarSystem;
    [SerializeField] private BallSystem ballSystem;
    [SerializeField] private BasketTrigger basketTrigger;
    [SerializeField] private BasketTrigger backBoardTrigger;

    [Header("Shot Timer")]
    [SerializeField] private bool isTimerRunning = false;
    [SerializeField] private float timerDuration = 1f;
    [SerializeField] private float currentTimer = 0f;

    public event Action OnTimerEnd;

    private void Start()
    {
        //if (InputManager.Instance != null)
        //{
        //    InputManager.Instance.OnClickStartNotOverUI += OnClickStartNotOverUI;
        //    InputManager.Instance.OnClickCanceledNotOverUI += OnClickCanceledNotOverUI;
        //}

        //if (UIGameplay.Instance != null)
        //    UIGameplay.Instance.OnCooldownEnd += OnCooldownEnd;

        //if (basketTrigger != null)
        //    basketTrigger.OnTriggered += OnBasketEnter;

        //if (backBoardTrigger != null)
        //    backBoardTrigger.OnTriggered += OnBackBoardTriggered;

        //if (ballSystem != null)
        //    ballSystem.OnBallHitFloor += OnBallHitFloor;

        InitializeGame();
    }

    private void OnDestroy()
    {
        //InputManager inputManager = InputManager.TryGetInstance();
        //if (inputManager != null)
        //{
        //    inputManager.OnClickStartNotOverUI -= OnClickStartNotOverUI;
        //    inputManager.OnClickCanceledNotOverUI -= OnClickCanceledNotOverUI;
        //}

        //UIGameplay uIGameplay = UIGameplay.TryGetInstance();
        //if (uIGameplay != null)
        //    uIGameplay.OnCooldownEnd -= OnCooldownEnd;

        //if (basketTrigger != null)
        //    basketTrigger.OnTriggered -= OnBasketEnter;

        //if (backBoardTrigger != null)
        //    backBoardTrigger.OnTriggered -= OnBackBoardTriggered;

        //if (ballSystem != null)
        //    ballSystem.OnBallHitFloor -= OnBallHitFloor;
    }
    private void Update()
    {
        if (!isPossibleToShoot)
            return;

        HandleTimer();
    }

    private void InitializeGame()
    {
        currentPositionIndex = 0;
        SetShotRange(currentPositionIndex);
    }

    private void HandleTimer()
    {
        if (!isTimerRunning)
            return;

        currentTimer += Time.deltaTime;

        if (currentTimer >= timerDuration)
            OnTimerEnd?.Invoke();
    }

    private void OnBackBoardTriggered()
    {

    }

    private void OnBasketEnter()
    {
        hasScored = true;
        if (lastShotType == ShotType.PerfectShot)
            playerPoints += 3;
        else if(lastShotType == ShotType.HighShot)
            playerPoints += 2;


        UIGameplay.Instance.IncreaseScore(true, playerPoints);
    }

    private void OnBallHitFloor()
    {
        fillBarSystem.ResetValue();

        if (hasScored)
        {
            scoreCount++;
            if (scoreCount % 4 == 0 && currentPositionIndex < shotRanges.Count - 1)
                currentPositionIndex++;
        }

        SetShotRange(currentPositionIndex);
        isPossibleToShoot = true;
        hasScored = false;
    }

    private void SetShotRange(int positionIndex)
    {
        var range = shotRanges[positionIndex];
        //fillBarSystem.SetShotRange(range.perfectMin, range.highMin);
    }

    private void OnClickStartNotOverUI()
    {
        if (isPossibleToShoot)
            isTimerRunning = true;
    }

    private void OnClickCanceledNotOverUI()
    {
        //if (!isPossibleToShoot)
        //    return;

        //isPossibleToShoot = false;
        //currentTimer = timerDuration;

        //lastShotType = shotRanges[currentPositionIndex].GetShotType(fillBarSystem.GetFillAmount());
        //print($" shotType is {lastShotType}");

        //ballSystem.ShootBall(lastShotType);
    }
    private void OnCooldownEnd()
    {
        isPossibleToShoot = true;
    }
}
