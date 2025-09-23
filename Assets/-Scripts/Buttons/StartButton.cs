using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public string sceneName = "The shed";

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
