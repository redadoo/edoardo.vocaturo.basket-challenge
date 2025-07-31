using UnityEngine;
using UIScript;

/// <summary>
/// Controls the behavior of AI enemy shooters.
/// </summary>
public class EnemyShooterController : ShooterController
{
    [Header("Shot Timer Settings")]
    [SerializeField] private float timerDuration = 1f;
    [SerializeField] private float currentTimer = 0f;

    [Header("Difficulty Configuration")]
    [SerializeField] private EnemyDifficultySO difficultySO;

    private void Update()
    {
        HandleShotTimer();
    }

    public override void Init(ShotInfoSO shotInfo)
    {
        isPlayer = false;
        difficultySO = GameManager.Instance.GetCurrentCampType().enemyDifficulty;

        base.Init(shotInfo);
    }

    /// <summary>
    /// Handles the timer logic for automated shooting.
    /// </summary>
    private void HandleShotTimer()
    {
        if (state != ShooterState.Dribbling || difficultySO == null)
            return;

        currentTimer += Time.deltaTime;

        if (currentTimer >= timerDuration)
        {
            shotType = difficultySO.GetRandomShotType();
            ballSystem.ShootBall(shotType, currentShotInfo);

            timerDuration = Random.Range(difficultySO.minTimerDuration, difficultySO.maxTimerDuration);
            currentTimer = 0f;
            state = ShooterState.Shot;
        }
    }
}
