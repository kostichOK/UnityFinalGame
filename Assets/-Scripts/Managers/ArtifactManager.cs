using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance;

    private int collectedArtifacts = 0;
    public int artifactsToWin = 3; // сколько нужно собрать для победы

    public GameObject winScreenPrefab; // префаб экрана победы
    private GameObject winScreenInstance;
    public AudioSource audioSource1;

    private void Awake()
    {
        // Синглтон, чтобы был только один менеджер
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Метод для вызова при подборе артефакта
    public void CollectArtifact()
    {
        collectedArtifacts++;
        Debug.Log("Собрано артефактов: " + collectedArtifacts);

        if (collectedArtifacts >= artifactsToWin)
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        audioSource1.Stop();
        Debug.Log("You win");
        SceneManager.LoadScene(4);
    }
}