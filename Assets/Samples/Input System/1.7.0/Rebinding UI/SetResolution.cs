using TMPro;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    public TMP_Dropdown m_Dropdown;

    private void Awake()
    {
        // Ensure m_Dropdown is assigned in the inspector or through GetComponent if it's null.
        if (m_Dropdown == null)
        {
            m_Dropdown = GetComponent<TMP_Dropdown>();
        }

        // Add listener for when the value of the Dropdown changes, to take action.
        m_Dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(m_Dropdown); });
    }

    // Method to handle the value change event of the dropdown.
    public void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        string resolution = dropdown.options[dropdown.value].text;

        switch (resolution)
        {
            case "1920x1080":
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                break;
            case "1366 x 768 (Laptop)":
                Screen.SetResolution(1366, 768, FullScreenMode.FullScreenWindow);
                break;
            case "1280x1024":
                Screen.SetResolution(1280, 1024, FullScreenMode.FullScreenWindow);
                break;
            case "800x600":
                Screen.SetResolution(800, 600, FullScreenMode.FullScreenWindow);
                break;
            default:
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                break;
        }
    }
}
