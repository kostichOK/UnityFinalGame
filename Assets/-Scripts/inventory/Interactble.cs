using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string requiredName; // имя предмета, который нужен (например "Key1")

    public bool TryUse(string itemName)
    {
        if (itemName == requiredName)
        {
            Debug.Log($"{requiredName} использован на {gameObject.name}");
            return true;
        }
        else
        {
            Debug.Log($"Предмет {itemName} не подходит для {gameObject.name}");
            return false;
        }
    }
}
