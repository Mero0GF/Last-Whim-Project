using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string filename;
    
    public GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    //indica que pode ser referenciada publicamente mas so pode modificar
    //a instancia de forma privada
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found more than one Data persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, filename);
    }
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        //update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;

        //load the game, which will use that profile, updating our game accordingly
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        this.gameData.sceneName = "Beach";
        this.gameData.playerSpawnPosition.x = -4.53f;
        this.gameData.playerSpawnPosition.y = -8.0f;
        this.gameData.playerPersistentData.hasSword = false;
        this.gameData.playerPersistentData.beachCutscenePlayed = false;
        this.gameData.playerPersistentData.firstBossDone = false;
        this.gameData.playerPersistentData.lastBossDone = false;
        this.gameData.swordSpawnPosition = Vector2.zero;
        dataHandler.Save(this.gameData, selectedProfileId);
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load(selectedProfileId);

        if(this.gameData == null)
        {
            Debug.Log("No data was found. A new Game needs to be started before data can be loaded.");
            return;
        }
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new Game needs to be started before data can be loaded.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
    }

    /*public void OnApplicationQuit()
    {
        SaveGame();
    }*/

    //encontrar os scripts que utilizam a interface IDataPersistence e retornar uma lista desses scripts
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {               
        return dataHandler.LoadAllProfiles();
    }

    public string SceneName()
    {
        return this.gameData.sceneName;
    }

    public void ChangeLastSceneIndex(int menuSceneIndex)
    {
        this.gameData.lastSceneIndex = menuSceneIndex;
        dataHandler.Save(gameData, selectedProfileId);
    }
}
