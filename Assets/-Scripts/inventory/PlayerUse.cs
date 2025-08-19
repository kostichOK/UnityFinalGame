using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private Inventory inventory;

    private void Update()
    {
        // ЛКМ — использовать предмет
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, interactMask))
            {
                Interactable interact = hit.collider.GetComponent<Interactable>();
                if (interact != null)
                {
                    // Берём имя предмета, который выбран в инвентаре
                    string itemName = inventory.GetItemName(inventory.currentIndex);

                    if (!string.IsNullOrEmpty(itemName))
                    {
                        bool used = interact.TryUse(itemName);
                        if (used)
                        {
                            inventory.RemoveItem(inventory.currentIndex);
                        }
                    }
                    else
                    {
                        Debug.Log("В руках нет предмета!");
                    }
                }
            }
        }
    }
}