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
    }

    private void OnCooldownEnd()
    {
        playerShooter.Init(GetShotRange(0));
        enemyShooter.Init(GetShotRange(0));
    }

    public ShotInfoSO GetShotRange(int index) =>
        shotInfos[index];

}
