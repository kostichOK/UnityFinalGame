using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public GameObject mapButton; // кнопка для карты
    [SerializeField] ButtonsSO buttonSO; // состояние карты
    private bool playerInside = false;

    // Метод для динамического присваивания кнопки
    public void SetMapButton(GameObject mapBtn)
    {
        mapButton = mapBtn;

        // Если игрок уже внутри зоны при присвоении кнопки, включаем её сразу
        if (playerInside && mapButton != null)
        {
            mapButton.SetActive(true);
            buttonSO.mapActive = true;
            buttonSO.closeMap = false;
        }
    }

    private void Start()
    {
        // Проверяем, находится ли игрок уже внутри зоны
        Collider playerCol = GameObject.FindWithTag("Player")?.GetComponent<Collider>();
        if (playerCol != null)
        {
            if (GetComponent<Collider>().bounds.Intersects(playerCol.bounds))
            {
                playerInside = true;

                // Если кнопка уже присвоена, включаем её
                if (mapButton != null)
                {
                    mapButton.SetActive(true);
                    buttonSO.mapActive = true;
                    buttonSO.closeMap = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (mapButton != null)
            {
                mapButton.SetActive(true);
                buttonSO.mapActive = true;
                buttonSO.closeMap = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (mapButton != null)
            {
                mapButton.SetActive(false);
                buttonSO.mapActive = false;
                buttonSO.closeMap = true;
            }
        }
    }
}