using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [Header("Имя сцены для загрузки")]
    public string sceneName = "The shead"; 
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
