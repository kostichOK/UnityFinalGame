using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private HoldItem holdItem; // наш скрипт для предметов в руках
    public float distance = 3f;
    public ItemInspection inspection;

    public GameObject cursorNormal;
    public GameObject cursorSee;

    private void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // центр экрана
        if (Physics.Raycast(ray, out RaycastHit hit, distance, interactMask))
        {
            cursorNormal.SetActive(false);
            cursorSee.SetActive(true);
            if (Input.GetMouseButtonDown(0))
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
                        cursorNormal.SetActive(true);
                        cursorSee.SetActive(false);
                        hit.collider.GetComponent<Chest>().Interact();
                    }
                }
            }
                
        }
    }
}