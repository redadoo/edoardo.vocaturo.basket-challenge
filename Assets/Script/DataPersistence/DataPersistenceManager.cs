using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utility;

namespace DataPersistance
{
    /// <summary>
    /// Central manager responsible for coordinating save/load operations across all objects
    /// </summary>
    public class DataPersistenceManager : PersistentSingleton<DataPersistenceManager>
    {
        [Header("File Storage Config")]
        [Tooltip("Name of the file used to store persistent data.")]
        [SerializeField] private string fileName;

        [Tooltip("Enable or disable basic encryption for stored data.")]
        [SerializeField] private bool useEncryption;

        [Tooltip("Enable or disable saving and loading.")]
        [SerializeField] private bool save;

        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileDataHandler dataHandler;

        private void OnEnable()
        {
            LoadingSceneManager.Instance.OnSceneChange += OnSceneChange;
        }

        public void Start()
        {
            if (save)
            {
                this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
                this.dataPersistenceObjects = FindAllDataPersistenceObjects();
                LoadGame();
            }
        }

        /// <summary>
        /// Automatically saves the game data when the application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            if (save) SaveGame();
        }

        /// <summary>
        /// Creates a new instance of <see cref="GameData"/> with default values.
        /// Used when starting a new game or when no saved data is found.
        /// </summary>
        public void NewGame()
        {
            if (save)
                this.gameData = new GameData();
        }

        /// <summary>
        /// Loads game data from disk and distributes it to all data persistence components.
        /// </summary>
        public void LoadGame()
        {
            if (save)
            {
                this.gameData = dataHandler.Load();

                if (this.gameData == null)
                {
                    Debug.Log("No data was found. Initializing data to defaults");
                    NewGame();
                }

                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                {
                    dataPersistenceObj.LoadData(gameData);
                }
            }
        }

        /// <summary>
        /// Collects data from all data persistence components and writes it to disk.
        /// </summary>
        public void SaveGame()
        {
            if (save)
            {
                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                    dataPersistenceObj.SaveData(ref gameData);

                dataHandler.Save(gameData);
            }
        }

        /// <summary>
        /// Finds and returns all active MonoBehaviours that implement <see cref="IDataPersistence"/>.
        /// </summary>
        /// <returns>A list of objects participating in data persistence.</returns>
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        /// <summary>
        /// Reloads game data when returning to the main menu scene.
        /// </summary>
        private void OnSceneChange(object sender, Scene e)
        {
            if (e == Scene.MainMenu)
                LoadGame();
        }

    }

}