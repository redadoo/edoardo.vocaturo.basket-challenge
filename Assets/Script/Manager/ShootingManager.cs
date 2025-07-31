using System.Collections.Generic;
using UnityEngine;
using UIScript;

/// <summary>
/// Manages the shooting phase of the game 
/// </summary>
public class ShootingManager : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [Tooltip("List of shot information for different shooting positions or difficulty levels.")]
    [SerializeField] private List<ShotInfoSO> shotInfos;

    [Header("References")]
    [Tooltip("Reference to the player's shooter controller.")]
    [SerializeField] private PlayerShooterController playerShooter;

    [Tooltip("Reference to the enemy's shooter controller.")]
    [SerializeField] private EnemyShooterController enemyShooter;

    private void Start()
    {
        if (UICountdown.Instance != null)
            UICountdown.Instance.OnCooldownEnd += OnCooldownEnd;

        if (UIGameTimer.Instance != null)
            UIGameTimer.Instance.OnGameEnd += OnGameEnd;
    }


    /// <summary>
    /// Called when the game ends. Resets both player and enemy shooters.
    /// </summary>
    private void OnGameEnd()
    {
        if (shotInfos.Count == 0)
            return;

        ShotInfoSO initialShot = shotInfos[0];

        playerShooter.SetShotInfo(initialShot);
        enemyShooter.SetShotInfo(initialShot);

        playerShooter.ResetValue();
        enemyShooter.ResetValue();

        GameManager.Instance.SetScores(playerShooter.points, enemyShooter.points);
    }

    /// <summary>
    /// Called when the start cooldown ends. Initializes both shooters.
    /// </summary>
    private void OnCooldownEnd()
    {
        if (shotInfos.Count == 0)
            return;

        ShotInfoSO initialShot = shotInfos[0];

        playerShooter.Init(initialShot);
        enemyShooter.Init(initialShot);
    }

    /// <summary>
    /// Gets the next shot range in the progression based on the current shot info.
    /// </summary>
    /// <returns>The next <see cref="ShotInfoSO"/> if available, otherwise the last one in the list.</returns>
    public ShotInfoSO GetNextShotRange(ShotInfoSO shotInfo)
    {
        int index = shotInfos.IndexOf(shotInfo);

        if (index >= 0 && index < shotInfos.Count - 1)
            return shotInfos[index + 1];

        return shotInfos.Count > 0 ? shotInfos[^1] : null;
    }

    /// <summary>
    /// Determines whether the player currently has more points than the enemy.
    /// </summary>
    /// <returns><c>true</c> if the player is winning; otherwise, <c>false</c>.</returns>
    public bool IsPlayerWinner() =>
        playerShooter.points > enemyShooter.points;
}
