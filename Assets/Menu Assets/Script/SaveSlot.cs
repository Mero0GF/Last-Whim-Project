using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]

    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDatatContent;
    [SerializeField] private TextMeshProUGUI lastSaveLocation;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();   
    }

    public void SetData(GameData data)
    {
        //there's no data for this profileId
        if(data == null)
        {
            noDataContent.SetActive(true);
            hasDatatContent.SetActive(false);
        }
        //there's data for this profileId
        else
        {
            noDataContent.SetActive(false);
            hasDatatContent.SetActive(true);

            lastSaveLocation.text = data.GetNameLocation();

        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }

    public string GetLastSavedLocation()
    {
        string sceneName = lastSaveLocation.text;
        return sceneName;
    }
}
