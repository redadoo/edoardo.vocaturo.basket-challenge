using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DataPersistenceManager : PersistentSingleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    [SerializeField] private bool save;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void OnEnable()
    {
        LoadingSceneManager.instance.OnSceneChange += OnSceneChange;
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
    public void NewGame()
    {
        if (save)
        {
            this.gameData = new GameData();
        }
    }

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

    public void SaveGame()
    {
        if (save)
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                dataPersistenceObj.SaveData(ref gameData);

            dataHandler.Save(gameData);
        }
    }

    private void OnApplicationQuit()
    {
        if (save)
            SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    private void OnSceneChange(object sender, Scene e)
    {
        if (e == Scene.MainMenu)
            LoadGame();
    }

}
