using UnityEngine;

public class Interactble : MonoBehaviour
{
    public string requiredKeyName; // имя ключа (например, "Key1")

    private bool isOpen = false;

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        Debug.Log("Дверь открыта ключом: " + requiredKeyName);

        // Здесь добавь анимацию или просто открой дверь
        transform.Rotate(0, 90, 0); // временно — просто поворот
    }
}
