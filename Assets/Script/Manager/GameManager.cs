using UnityEngine;
using System;

public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
{
    public int gold { get; private set; } = 0;
    public int money { get; private set; } = 0;


    public event Action OnMoneyChange;

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

    public void IncreaseMoney(int toAdd)
    {
        if (money > int.MaxValue - toAdd)
            money = int.MaxValue;
        else
            money += toAdd;
        OnMoneyChange?.Invoke();
    }

    public void DecreaseMoney(int toRemove)
    {
        money = Mathf.Max(0, money - toRemove);
        OnMoneyChange?.Invoke();
    }

}
