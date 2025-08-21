using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private HoldItem holdItem; // наш скрипт для предметов в руках
    public ItemInspection inspection;

    private void Update()
    {
        // ЛКМ — использовать предмет
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // центр экрана
            if (Physics.Raycast(ray, out RaycastHit hit, distance, interactMask))
            {
                // Проверяем, есть ли в руках предмет
                GameObject currentItem = holdItem.GetCurrentItem();
                if (currentItem == null) return;

                // Проверяем, что объект, в который тыкаем — дверь
                Interactble door = hit.collider.GetComponent<Interactble>();
                if (door != null)
                {
                    // Если у двери ключ совпадает с названием предмета
                    if (door.requiredKeyName == currentItem.name)
                    {
                        currentItem.SetActive(false);
                        door.Open();
                        holdItem.Release(); // выбрасываем/убираем ключ (по желанию)
                        inspection.handOcuped = false;
                    }
                }
            }
        }
    }
}