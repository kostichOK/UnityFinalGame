using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public string sceneName = "Cementery";

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
}
