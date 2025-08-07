using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] ButtonsSO buttonSO;
    public GameObject mapImage;
    bool mapIsActive = false;

    private void Update()
    {
        if (buttonSO.mapActive == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (mapIsActive == false)
                {
                    mapImage.SetActive(true);
                    mapIsActive = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    mapImage.SetActive(false);
                    mapIsActive = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
        if (buttonSO.closeMap == true)
        {
            mapImage.SetActive(false);
            mapIsActive = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

                
    public void ToIndustry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
