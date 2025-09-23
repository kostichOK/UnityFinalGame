using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void QuitGame()
    {
        #if UNITY_EDITOR
        
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // якщо гра запущена через €рлик (build) Ч закриваЇмо програму
                Application.Quit();
        #endif
    }
}
