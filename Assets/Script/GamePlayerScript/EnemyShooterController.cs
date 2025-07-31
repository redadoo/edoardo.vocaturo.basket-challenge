using UnityEngine;

/// <summary>
/// Controls the behavior of AI enemy shooters. Handles automated shot timing and execution.
/// </summary>
public class EnemyShooterController : ShooterController
{
    [Header("Shot Timer")]
    [SerializeField] private float timerDuration = 1f;
    [SerializeField] private float currentTimer = 0f;
    [SerializeField] private EnemyDifficultySO difficultySO;

    private void Update()
    {
        HandleShotTimer();
    }

    public override void Init(ShotInfoSO shotInfo)
    {
        isPlayer = false;
        SetDifficulty();
        base.Init(shotInfo);
    }

    protected override void OnBallScored()
    {
        base.OnBallScored();

        points += pointScoredLastTime + bonusPoints;

        UIFeedback.Instance.ShowScore(isPlayer, pointScoredLastTime);
        UIGameplay.Instance.UpdateScore(isPlayer, points);
    }

    /// <summary>
    /// Handles the shooting timer. Automatically triggers a shot after the timer elapses.
    /// </summary>
    private void HandleShotTimer()
    {
        if (state != ShooterState.Dribbling)
            return;

        currentTimer += Time.deltaTime;

        if (currentTimer >= timerDuration)
        {
            shotType = difficultySO.GetRandomShotType();
            ballSystem.ShootBall(shotType, currentShotInfo);

            timerDuration = Random.Range(difficultySO.minTimerDuration, difficultySO.maxnTimerDuration);
            currentTimer = 0f;
            state = ShooterState.Shot;
        }
    }

    private void SetDifficulty()
    {
        difficultySO = GameManager.Instance.GetCurrentCampType().enemyDifficulty;
    }
}
