using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel; 

    
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

   
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

   
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
