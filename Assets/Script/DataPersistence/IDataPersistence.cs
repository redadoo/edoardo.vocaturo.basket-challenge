namespace DataPersistance
{
    /// <summary>
    /// Interface for components that need to save and load data.
    /// </summary>
    public interface IDataPersistence
    {
        /// <summary>
        /// Loads relevant data from the provided <see cref="GameData"/> object into the implementing component.
        /// Called during the game's loading process.
        /// </summary>
        /// <param name="data">The <see cref="GameData"/> object containing saved values.</param>
        void LoadData(GameData data);

        /// <summary>
        /// Saves relevant data from the implementing component into the provided <see cref="GameData"/> object.
        /// Called during the game's saving process.
        /// </summary>
        /// <param name="data">A reference to the <see cref="GameData"/> object to be updated.</param>
        void SaveData(ref GameData data);
    }
}
