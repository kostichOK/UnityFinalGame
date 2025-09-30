using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void ToMenu()
    {
        SceneManager.LoadScene(5);
    }

    public void ClickStartMenu()
    {
        SceneManager.LoadScene(0);
    }
}
