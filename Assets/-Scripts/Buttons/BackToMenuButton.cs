using UnityEngine;

public class CloseSettingsButton : MonoBehaviour
{
    [Header("Панель с настройками")]
    public GameObject settingsPanel;
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}
