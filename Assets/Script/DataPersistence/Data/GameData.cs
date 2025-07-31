using System;

namespace DataPersistance
{
    /// <summary>
    /// Represents the persistent data structure for saving and loading game state.
    /// </summary>
    [Serializable]
    public class GameData
    {
        public string username;
        public int money;
        public int gold;

        /// <summary>
        /// Initializes a new instance of <see cref="GameData"/> with default values.
        /// </summary>
        public GameData() 
        {
            username = "user_1234";
            money = 100000;
            gold = 0;
        }
    }
}
