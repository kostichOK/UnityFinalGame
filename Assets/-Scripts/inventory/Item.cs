using UnityEngine.SceneManagement;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string objectID;
    private Rigidbody rb;
    private bool isDropped = false;
    private bool isHeld = false; // предмет у игрока?

    private static readonly System.Collections.Generic.Dictionary<string, Vector3> savedPositions =
        new System.Collections.Generic.Dictionary<string, Vector3>();
    private static readonly System.Collections.Generic.HashSet<string> pickedUpItems =
        new System.Collections.Generic.HashSet<string>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    [System.Obsolete]
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [System.Obsolete]
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [System.Obsolete]
    private void Start()
    {
        ApplySavedState();
    }

    [System.Obsolete]
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySavedState();
    }

    [System.Obsolete]
    private void ApplySavedState()
    {
        // Ставим на сохранённую позицию только если предмет не в руках
        if (!isHeld && savedPositions.ContainsKey(objectID))
        {
            // Если предмет был подобран и находится в руках — отключаем его в сцене
            if (pickedUpItems.Contains(objectID))
            {
                gameObject.SetActive(false);
                return;
            }

            // Если предмет был оставлен на полу (есть сохранённая позиция)
            if (savedPositions.ContainsKey(objectID))
            {
                transform.position = savedPositions[objectID]; // Если убрать будет лежать на полу но не удалится из сцены
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }


            }
        }
    }

    [System.Obsolete]
    private void Update()
    {
        if (isDropped && rb != null)
        {
            if (rb.velocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f)
            {
                savedPositions[objectID] = transform.position;
                isDropped = false;
            }
        }
    }

    [System.Obsolete]
    public void PickUp()
    {
        pickedUpItems.Add(objectID);
        isHeld = true; // теперь предмет у игрока
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    [System.Obsolete]
    public void Drop(Vector3 newPos)
    {
        isHeld = false; // предмет больше не в руках
        transform.position = newPos;
        pickedUpItems.Remove(objectID);
        isDropped = true;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}