using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Image[] slotImages; // UI картинки слотов
    private string[] slotNames;   // Названия предметов
    private Sprite[] slotIcons;   // Иконки предметов
    private bool[] isOccupied;    // Статус слота

    public int currentIndex = 0; // Индекс выбранного предмета (-1 = ничего не выбрано)

    public GameObject firstSelected;
    public GameObject secondSelected;

    private void Awake()
    {
        int size = slotImages.Length;
        slotNames = new string[size];
        slotIcons = new Sprite[size];
        isOccupied = new bool[size];

        for (int i = 0; i < size; i++)
        {
            slotNames[i] = null;
            slotIcons[i] = null;
            isOccupied[i] = false;

            if (slotImages[i] != null)
            {
                slotImages[i].sprite = null;
                slotImages[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Перебираем цифры от 1 до количества слотов
        for (int i = 0; i < slotNames.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);

                // если хочешь, чтобы объекты firstSelected / secondSelected работали:
                if (i == 0)
                {
                    firstSelected.SetActive(true);
                    secondSelected.SetActive(false);
                }
                else if (i == 1)
                {
                    firstSelected.SetActive(false);
                    secondSelected.SetActive(true);
                }
            }
        }
    }

    public bool AddItem(string name, Sprite icon)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (!isOccupied[i])
            {
                slotNames[i] = name;
                slotIcons[i] = icon;
                slotImages[i].sprite = icon;
                slotImages[i].gameObject.SetActive(true);
                isOccupied[i] = true;

                Debug.Log($"Добавлен {name} в слот {i + 1}");
                return true;
            }
        }
        Debug.Log("Инвентарь полон!");
        return false;
    }

    private void SelectSlot(int index)
    {
        if (index >= 0 && index < slotNames.Length && isOccupied[index])
        {
            currentIndex = index;
            Debug.Log("Выбран предмет: " + slotNames[currentIndex]);
        }
        else
        {
            currentIndex = -1;
            Debug.Log("Слот пустой!");
        }
    }

    // Используем предмет
    public bool UseItem(string requiredName)
    {
        if (currentIndex >= 0 && isOccupied[currentIndex])
        {
            string itemInHand = slotNames[currentIndex];
            Debug.Log($"Сейчас выбран предмет: {itemInHand}");

            if (itemInHand == requiredName)
            {
                Debug.Log($"Использован {itemInHand}!");
                RemoveItem(currentIndex); // удаляем именно этот слот
                return true;
            }
            else
            {
                Debug.Log($"{itemInHand} не подходит для {requiredName}");
                return false;
            }
        }

        Debug.Log("Нет выбранного предмета!");
        return false;
    }

    public string GetItemName(int index)
    {
        if (index >= 0 && index < slotNames.Length && isOccupied[index])
            return slotNames[index];
        return null;
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slotNames.Length && isOccupied[index])
        {
            Debug.Log($"Удалён {slotNames[index]} из слота {index}");
            slotNames[index] = null;
            slotIcons[index] = null;
            isOccupied[index] = false;
            slotImages[index].sprite = null;
            slotImages[index].gameObject.SetActive(false);

            if (currentIndex == index)
                currentIndex = -1; // сброс выбора, если удалили выбранный предмет
        }
    }

    public int SlotCount => slotNames.Length;
}