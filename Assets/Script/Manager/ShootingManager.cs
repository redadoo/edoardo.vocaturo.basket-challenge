using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootingManager : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [SerializeField] private List<ShotInfoSO> shotInfos;

    [Header("References")]
    [SerializeField] private PlayerShooterController playerShooter;
    [SerializeField] private EnemyShooterController enemyShooter;

    private void Start()
    {
        if (UIGameplay.Instance != null)
            UIGameplay.Instance.OnCooldownEnd += OnCooldownEnd;

        if (UIGameTimer.Instance != null)
            UIGameTimer.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd()
    {
        playerShooter.SetShotInfo(GetShotRange(0));
        enemyShooter.SetShotInfo(GetShotRange(0));

        playerShooter.ResetValue();
        enemyShooter.ResetValue();
    }

    private void OnCooldownEnd()
    {
        playerShooter.Init(GetShotRange(0));
        enemyShooter.Init(GetShotRange(0));
    }

    public ShotInfoSO GetShotRange(int index) =>
        shotInfos[index];

    public bool IsPlayerWinner() =>
        playerShooter.points > enemyShooter.points;

}
