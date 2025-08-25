using UnityEngine;

public class Interactble : MonoBehaviour
{
    public string requiredKeyName; // имя ключа (например, "Key1")
    public Animator animator;
    private bool isOpen = false;

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        Debug.Log("Дверь открыта ключом: " + requiredKeyName);
        animator.SetTrigger("OpenChest");
    }
}
