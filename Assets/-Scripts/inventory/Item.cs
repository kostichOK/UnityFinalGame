using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{
    public string objectID; // уникальный идентификатор
    private Rigidbody rb;

    private bool isHeld = false;
    private bool isDropped = false;

    // Словари для сохранения данных между сценами
    private static Dictionary<string, Vector3> itemPositions = new Dictionary<string, Vector3>();
    private static HashSet<string> heldItems = new HashSet<string>();
    private static HashSet<string> spawnedItems = new HashSet<string>(); // чтобы не создавать дубликаты

    private Vector3 startPos; // стартовая позиция в сцене

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(EnablePhysicsAfterSpawn());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ставим позицию
        ApplySavedState();
    }

    public IEnumerator EnablePhysicsAfterSpawn()
    {
        // Срабатывает только для ключей
        if (rb == null) yield break;

        // Ждём немного, чтобы Terrain и коллайдеры прогрузились
        yield return new WaitForSeconds(0.1f);

        if (!isHeld) // если предмет не в руках
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void Start()
    {
        // Если объект ещё не в руках, делаем его кинематиком на момент спавна
        if (!isHeld && rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        ApplySavedState(); // восстановление позиции
    }

    private void ApplySavedState()
    {
        // Если ключ в руках игрока, оставляем его там
        if (heldItems.Contains(objectID))
        {
            isHeld = true;
        }
        else
        {
            isHeld = false;

            // Если ключ был перемещён, ставим его на новую позицию
            if (itemPositions.TryGetValue(objectID, out Vector3 savedPos))
            {
                transform.position = savedPos;
            }
            else
            {
                // Если ключ не тронут — стартовая позиция
                transform.position = startPos;
            }

            // Настройка физики для нормального падения
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        if (!heldItems.Contains(objectID))
            gameObject.SetActive(true);
    }

    public void PickUp()
    {
        if (isHeld) return;

        isHeld = true;
        heldItems.Add(objectID);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public void Drop(Vector3 newPosition)
    {
        if (!isHeld) return;

        isHeld = false;
        heldItems.Remove(objectID);

        transform.position = newPosition;
        itemPositions[objectID] = newPosition;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        gameObject.SetActive(true); // ключ всегда видим
    }

    private void Update()
    {
        // Фиксируем позицию, если ключ лежит на полу и остановился
        if (!isHeld && rb != null && rb.velocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f)
        {
            itemPositions[objectID] = transform.position;
        }
    }

    public static bool HasPosition(string objectID, out Vector3 pos)
    {
        return itemPositions.TryGetValue(objectID, out pos);
    }
}