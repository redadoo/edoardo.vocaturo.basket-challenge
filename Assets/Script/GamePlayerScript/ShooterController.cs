using AudioSystem;
using System;
using UIScript;
using UnityEngine;

/// <summary>
/// Possible states for the shooter during gameplay.
/// </summary>
public enum ShooterState
{
    Idle,
    Dribbling,
    Charging,
    Shot,
    Scored
}

/// <summary>
/// Base class for controlling a shooter character (player or AI).
/// </summary>
public abstract class ShooterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected ShootingManager shootingManager;
    [SerializeField] protected BallSystem ballSystem;

    [Header("Shooter State")]
    [SerializeField] protected ShotType shotType;
    [SerializeField] protected ShooterState state;
    [SerializeField] protected ShotInfoSO currentShotInfo;
    [SerializeField] protected int currentPositionIndex;

    [Header("Scoring")]
    [SerializeField] protected int pointScoredLastTime;
    [SerializeField] protected int pointScored;
    [SerializeField] protected int bonusPoints;

    [Header("SoundData")]
    [SerializeField] protected SoundData basketSound;

    protected const int PerfectShotPoints = 3;
    protected const int HighShotPoints = 2;
    protected const int MaxShotsBeforeRangeIncrease = 3;
    protected bool isPlayer;
    public int points { get; protected set; }

    protected virtual void OnEnable()
    {
        if (ballSystem == null) return;
        
        ballSystem.OnBallHitFloor += OnBallHitFloor;
        ballSystem.OnBallScored += OnBallScored;
        ballSystem.OnBackboardHit += OnBackboardHit;
    }

    protected virtual void OnDestroy()
    {
        if (ballSystem == null) return;

        ballSystem.OnBallHitFloor -= OnBallHitFloor;
        ballSystem.OnBallScored -= OnBallScored;
        ballSystem.OnBackboardHit -= OnBackboardHit;
    }

    /// <summary>
    /// Called when the ball hits the floor. Handles shot range progression and resets state.
    /// </summary>
    protected virtual void OnBallHitFloor()
    {
        if (state == ShooterState.Idle) return;

        if (pointScored != 0 && pointScored % MaxShotsBeforeRangeIncrease == 0)
        {
            currentShotInfo = shootingManager.GetNextShotRange(currentShotInfo);
            pointScored = 0;
        }

        if (state == ShooterState.Scored)
            SetTransform();

        state = ShooterState.Dribbling;
        bonusPoints = 0;
    }

    /// <summary>
    /// Called when the ball is successfully scored in the basket.
    /// Handles scoring logic, feedback, and state changes.
    /// </summary>
    protected virtual void OnBallScored()
    {
        HandleScoreCalculation();
        HandleScoreFeedback();
    }

    /// <summary>
    /// Handles the logic for calculating and updating the score based on shot type.
    /// </summary>
    protected virtual void HandleScoreCalculation()
    {
        switch (shotType)
        {
            case ShotType.PerfectShot:
                pointScoredLastTime = PerfectShotPoints;
                break;

            case ShotType.NormalShot:
                pointScoredLastTime = HighShotPoints;
                break;

            default:
                return;
        }

        state = ShooterState.Scored;
        pointScored++;
        points += pointScoredLastTime + bonusPoints;
    }

    /// <summary>
    /// Handles UI and sound feedback after scoring.
    /// </summary>
    protected virtual void HandleScoreFeedback()
    {
        UIFeedback.Instance.ShowScore(isPlayer, pointScoredLastTime + bonusPoints, shotType == ShotType.PerfectShot);
        UIScore.Instance.UpdateScore(isPlayer, points);

        bonusPoints = 0;
        pointScoredLastTime = 0;

        SoundManager.Instance.CreateSound()
            .WithSoundData(basketSound)
            .Play();
    }

    /// <summary>
    /// Called when the ball hits the backboard. Applies bonus points if valid.
    /// </summary>
    protected virtual void OnBackboardHit(BackboardBonus backboard)
    {
        if (shotType != ShotType.NormalShot) return;

        if (!backboard.wasHit)
        {
            bonusPoints = backboard.GetBonusValue();
            backboard.wasHit = true;
        }
    }

    /// <summary>
    /// Sets the transform of the shooter based on current shot configuration and state.
    /// </summary>
    protected void SetTransform()
    {
        currentShotInfo.SetTransform(transform, pointScored, isPlayer);
    }


    /// <summary>
    /// Initializes the shooter with a given shot configuration.
    /// </summary>
    public virtual void Init(ShotInfoSO shotInfo)
    {
        currentShotInfo = shotInfo;
        state = ShooterState.Dribbling;
        SetTransform();
    }

    /// <summary>
    /// Resets the shooter's state.
    /// </summary>
    public virtual void ResetValue()
    {
        state = ShooterState.Idle;
        pointScored = 0;
        SetTransform();
        ballSystem.ResetBall();
    }

    /// <summary>
    /// Sets the shooter's current shot information.
    /// </summary>
    public void SetShotInfo(ShotInfoSO shotInfo)
    {
        currentShotInfo = shotInfo;
    }
}
