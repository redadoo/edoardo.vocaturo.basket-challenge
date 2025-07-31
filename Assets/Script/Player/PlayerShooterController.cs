using UnityEngine;
using UIScript;

/// <summary>
/// Handles player-controlled shooting behavior, including input events, charge timing, 
/// UI feedback, and interaction with the shot mechanics.
/// </summary>
public class PlayerShooterController : ShooterController
{
    [Header("References")]
    [SerializeField] private FillBarSystem fillBarSystem;
    [SerializeField] private TrailSystem trailSystem;
    [SerializeField] private UIFireball uiFireball;

    [Header("Shot Timer")]
    [SerializeField] private float timerDuration = 1f;
    [SerializeField] private float currentTimer = 0f;

    private bool isBallOnFire;

    protected override void OnEnable()
    {
        isPlayer = true;

        base.OnEnable();

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnClickStartNotOverUI += OnClickStartNotOverUI;
            InputManager.Instance.OnClickCanceledNotOverUI += OnClickCanceledNotOverUI;
        }
       
        if (uiFireball != null)
        {
            uiFireball.OnFireballStart += OnStartFireBall;
            uiFireball.OnFireballEnd += OnEndFireBall;
        }
    }

    private void Update()
    {
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

        SetComponentState(true);
        fillBarSystem.SetShotRange(currentShotInfo);
    }

    /// <summary>
    /// Called when the ball hits the floor. Resets components and shows miss feedback.
    /// </summary>
    protected override void OnBallHitFloor()
    {
        if (state != ShooterState.Scored)
        {
            uiFireball.OnShotMissed();
            isBallOnFire = false;
        }

        base.OnBallHitFloor();

        fillBarSystem.SetShotRange(currentShotInfo);
        fillBarSystem.ResetValue();
        currentTimer = 0f;
    }

    protected override void OnBallScored()
    {
        base.OnBallScored();

        if (isBallOnFire) pointScoredLastTime *= 2;
        else uiFireball.OnShotMade(pointScoredLastTime);

        points += pointScoredLastTime + bonusPoints;

        UIFeedback.Instance.ShowScore(isPlayer, pointScoredLastTime + bonusPoints);
        UIGameplay.Instance.UpdateScore(isPlayer, points);
    }

    /// <summary>
    /// Handles the countdown timer during shot charging. Shoots automatically when time is up.
    /// </summary>
    private void HandleTimer()
    {
        if (state != ShooterState.Charging)
            return;

        currentTimer += Time.deltaTime;

        if (currentTimer >= timerDuration)
        {
            SetComponentState(false);
            state = ShooterState.Shot;
            shotType = currentShotInfo.GetShotType(fillBarSystem.GetFillAmount());
            ballSystem.ShootBall(shotType, currentShotInfo);
            currentTimer = 0f;
        }
    }

    /// <summary>
    /// Triggered when the player begins a click (not over UI). Starts charging the shot.
    /// </summary>
    private void OnClickStartNotOverUI()
    {
        if (state == ShooterState.Dribbling)
        {
            state = ShooterState.Charging;
            SetComponentState(true);
        }
    }

    /// <summary>
    /// Triggered when the player releases a click (not over UI). Calculates shot type and shoots.
    /// </summary>
    private void OnClickCanceledNotOverUI()
    {
        if (state == ShooterState.Charging)
        {
            SetComponentState(false);
            state = ShooterState.Shot;
            shotType = currentShotInfo.GetShotType(fillBarSystem.GetFillAmount());
            ballSystem.ShootBall(shotType, currentShotInfo);
        }
    }

    /// <summary>
    /// Enables or disables visual components related to shot charging (fill bar, trail).
    /// </summary>
    /// <param name="active">Whether components should be active.</param>
    private void SetComponentState(bool active)
    {
        fillBarSystem.ChangeStatus(active);
        trailSystem.ChangeTrailState(active);
    }

    private void OnStartFireBall() => 
        isBallOnFire = true;
    private void OnEndFireBall() =>
        isBallOnFire = false;

    /// <summary>
    /// Resets the shooter's state and visual elements to default.
    /// </summary>
    public override void ResetValue()
    {
        base.ResetValue();
        SetComponentState(false);
    }
}
