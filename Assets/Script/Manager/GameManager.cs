using System.Collections.Generic;
using DataPersistance;
using UnityEngine;
using AudioSystem;
using Utility;
using System;

/// <summary>
/// Manages core game state such as money, match info, and audio.
/// </summary>
public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
{
    [Header("Match Info")]
    [SerializeField] private CampTypeSO currentCampType;
    [SerializeField] private List<CampTypeSO> availableCampTypes;

    [Header("Audio Settings")]
    [SerializeField] private SoundData startSound;
    [SerializeField] private SoundData mainMusic;

    public int playerLastMatchPoints { get; private set; }
    public int enemyLastMatchPoints { get; private set; }

    public int gold { get; private set; } = 0;
    public int money { get; private set; } = 0;

    public event Action OnMoneyChange;

    private void Start()
    {
        SoundManager.Instance.CreateSound()
            .WithSoundData(startSound)
            .Play();

        SoundManager.Instance.CreateSound()
            .WithSoundData(mainMusic)
            .Play();
    }

    /// <summary>
    /// Sets the last recorded match score.
    /// </summary>
    public void SetScores(int playerPoints, int enemyPoints)
    {
        playerLastMatchPoints = playerPoints;
        enemyLastMatchPoints = enemyPoints;
    }

    /// <summary>
    /// Sets the active match info based on the camp type index.
    /// </summary>
    public void SetMatchInfo(int index)
    {
        if (index < 0 || index >= availableCampTypes.Count)
        {
            Debug.LogWarning($"Invalid camp index: {index}");
            return;
        }

        currentCampType = availableCampTypes[index];
    }

    public CampTypeSO GetCurrentCampType()
    {
        return currentCampType;
    }

    public void CreateAndAssignCampType(int matchDuration, int difficultyIndex)
    {
        if (difficultyIndex < 0)
        {
            Debug.LogWarning($"Invalid difficulty index: {difficultyIndex}");
            return;
        }

        if (availableCampTypes.Count == 0)
        {
            Debug.LogWarning("No base CampTypeSO available to copy from.");
            return;
        }

        CampTypeSO baseCamp = availableCampTypes[^1];
        currentCampType = ScriptableObject.CreateInstance<CampTypeSO>();

        currentCampType.matchAdmissionFee = 250;
        currentCampType.enemyDifficulty = availableCampTypes[difficultyIndex].enemyDifficulty;
        currentCampType.matchDuration = matchDuration;

    }


    /// <summary>
    /// Returns whether the player has enough money to pay the selected match admission fee.
    /// </summary>
    public bool HasEnoughMoneyForFee()
    {
        return currentCampType != null && money >= currentCampType.matchAdmissionFee;
    }

    /// <summary>
    /// Changes the player's money amount and invokes the money change event.
    /// </summary>
    public void ChangeMoney(int amount)
    {
        if (amount == 0) return;

        
        if (amount > 0)
            money = (money > int.MaxValue - amount) ? int.MaxValue : money + amount;
        else
            money = Mathf.Max(0, money + amount);

        OnMoneyChange?.Invoke();
    }

    public void GiveMoneyReward()
    {
        ChangeMoney(currentCampType.matchRewardValue);
    }

    public void LoadData(GameData data)
    {
        money = data.money;
        gold = data.gold;

        OnMoneyChange?.Invoke();
    }

    public void SaveData(ref GameData data)
    {
        data.money = money;
        data.gold = gold;
    }
}
