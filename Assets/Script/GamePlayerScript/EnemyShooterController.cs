using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Playing,
}

public class EnemyShooterController : ShooterController
{
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private float toWait;

    [SerializeField] private bool isPressed;
    [SerializeField] private bool isTimerRunning = false;
    [SerializeField] private float timerDuration = 1f;
    [SerializeField] private float currentTimer = 0f;

    private void Update()
    {
        if (enemyState == EnemyState.Idle)
            return;

        HandleTimer();
    }

    public override void Init(ShotInfoSO shotInfo)
    {
        base.Init(shotInfo);

        enemyState = EnemyState.Playing;
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

        if (hasScored)
        {
            print("pantomima");
            transform.position = currentShotInfo.shotPositions[pointScored].transform.position;
            transform.rotation = currentShotInfo.shotPositions[pointScored].transform.rotation;
        }

        currentTimer = 0;
        isTimerRunning = false;
        isPossibleToShoot = true;
        hasScored = false;
        enemyState = EnemyState.Playing;
    }

    protected override void OnBallScored()
    {
        base.OnBallScored();
        hasScored = true;
        UIGameplay.Instance.IncreaseScore(false, points);
    }

    private void HandleTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= timerDuration)
        {
            shotType = (ShotType)Random.Range(0, 4);
            ballSystem.ShootBall(shotType, currentShotInfo);
            timerDuration = Random.Range(1.5f, 2.8f);
            enemyState = EnemyState.Idle;
        }
    }
}
