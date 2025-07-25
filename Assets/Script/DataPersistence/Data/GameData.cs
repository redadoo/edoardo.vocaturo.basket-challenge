using System;

[Serializable]
public class GameData
{
    public string username;
    public int money;
    public int gold;
    public GameData() 
    {
        username = "user_1234";
        money = 0;
        gold = 0;
    }
}
