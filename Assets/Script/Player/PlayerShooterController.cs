using UnityEngine;
using System;

public class PlayerShooterController : ShooterController
{
    [Header("Reference")]
    [SerializeField] private FillBarSystem fillBarSystem;
    [SerializeField] private TrailSystem trailSystem;

    [Header("Shot Timer")]
    [SerializeField] private bool isPressed;
    [SerializeField] private bool isTimerRunning = false;
    [SerializeField] private float timerDuration = 1f;
    [SerializeField] private float currentTimer = 0f;


    protected override void OnEnable()
    {
        base.OnEnable();

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnClickStartNotOverUI += OnClickStartNotOverUI;
            InputManager.Instance.OnClickCanceledNotOverUI += OnClickCanceledNotOverUI;
        }
    }

    private void Update()
    {
        if (!isPossibleToShoot)
            return;

        HandleTimer();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        InputManager inputManager = InputManager.TryGetInstance();
        if (inputManager != null)
        {
            inputManager.OnClickStartNotOverUI -= OnClickStartNotOverUI;
            inputManager.OnClickCanceledNotOverUI -= OnClickCanceledNotOverUI;
        }
    }

    public override void Init(ShotInfoSO shotInfo)
    {
        base.Init(shotInfo);

        trailSystem.ChangeTrailState(true);
        fillBarSystem.ChangeStatus(true);
        fillBarSystem.SetShotRange(currentShotInfo);
        transform.position = currentShotInfo.shotPositions[pointScored].transform.position;
        transform.rotation = currentShotInfo.shotPositions[pointScored].transform.rotation;
    }

    protected override void OnBallHitFloor()
    {
        if (pointScored != 0 && pointScored % 3 == 0)
        {
            currentPositionIndex++;
            currentShotInfo = shootingManager.GetShotRange(currentPositionIndex);
            pointScored = 0;
        }
    
        fillBarSystem.SetShotRange(currentShotInfo);
        if (hasScored)
        {
            transform.position = currentShotInfo.shotPositions[pointScored].transform.position;
            transform.rotation = currentShotInfo.shotPositions[pointScored].transform.rotation;
        }

        fillBarSystem.ResetValue();
        currentTimer = 0;
        isTimerRunning = false;
        isPossibleToShoot = true;
        hasScored = false;
    }

    protected override void OnBallScored()
    {
        base.OnBallScored();
        hasScored = true;
        UIGameplay.Instance.IncreaseScore(true, points);
    }

    private void HandleTimer()
    {
        if (!isTimerRunning)
            return;

        currentTimer += Time.deltaTime;

        if (currentTimer >= timerDuration)
        {
            trailSystem.ChangeTrailState(false);
            isTimerRunning = false;
            ballSystem.ShootBall(ShotType.PerfectShot, currentShotInfo);
            isPossibleToShoot = false;
            fillBarSystem.ChangeStatus(false);
            isPressed = false;
        }
    }

    private void OnClickStartNotOverUI()
    {
        if (!isPressed && isPossibleToShoot)
        {
            trailSystem.ChangeTrailState(true);
            fillBarSystem.ChangeStatus(true);
            isPressed = true;
            isTimerRunning = true;
        }
    }

    private void OnClickCanceledNotOverUI()
    {
        if (isPressed && isPossibleToShoot)
        {
            shotType = currentShotInfo.GetShotType(fillBarSystem.GetFillAmount());
            ballSystem.ShootBall(shotType, currentShotInfo);
            hasScored = false;
            isPossibleToShoot = false;
            fillBarSystem.ChangeStatus(false);
            trailSystem.ChangeTrailState(false);
            isPressed = false;
        }
    }
}