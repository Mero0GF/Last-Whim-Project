using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [Header("Menu Buttons")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    //[SerializeField] private Button continueGameButton;
    public GameData gameData;

    /*public void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }*/
    public void OnNewGameClicked(){
        /*//create a new game
        DataPersistenceManager.instance.NewGame();
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("Beach");
        */

        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void QuitGame(){

        Debug.Log("Quit!");
        Application.Quit();
      
    }

   /* public void OnContinueGameClicked()
    {
        //load saved game
        DataPersistenceManager.instance.LoadGame();
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync(gameData.sceneName);
    }*/
  
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }
    
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
