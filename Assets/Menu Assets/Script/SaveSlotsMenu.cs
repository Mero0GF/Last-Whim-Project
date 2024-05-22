using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] MainMenu menu;

    private SaveSlot[] saveSlots;
    public GameData data;
    private bool isLoadingGame = false;
    private string sceneName = "";
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        //update the selected proifle id to be used for data persistence
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!isLoadingGame)
        {
            //create a new game, which will initialize our data to a clean state
            DataPersistenceManager.instance.NewGame();
            //DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync("Beach");
            return;
        }
        //DataPersistenceManager.instance.LoadGame();        
        sceneName = DataPersistenceManager.instance.SceneName();
        DataPersistenceManager.instance.ChangeLastSceneIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void OnBackClicked()
    {
        menu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        //set this menu to be active
        this.gameObject.SetActive(true);

        //set mode
        this.isLoadingGame = isLoadingGame;

        //load all the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        //loop through each save slot in the UI and set the content appropriately
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu() 
    {
        //set this menu to be inactive
        this.gameObject.SetActive(false);

    }
}
