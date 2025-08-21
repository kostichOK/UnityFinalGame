using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [Header("Панель с настройками")]
    public GameObject settingsPanel;   
    public void ToggleSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}
