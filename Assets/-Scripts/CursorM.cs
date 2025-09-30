using UnityEngine;

public class CursorM : MonoBehaviour
{
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
