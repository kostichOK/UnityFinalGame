using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    public string objectID; // уникальный ID сундука

    // Статический словарь для хранения статуса
    private static Dictionary<string, bool> openedChests = new Dictionary<string, bool>();

    public bool isOpen = false;

    private Animator animator; // допустим, у сундука анимация
    private Collider chestCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        chestCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ApplySavedState();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySavedState();
    }

    private void ApplySavedState()
    {
        if (openedChests.TryGetValue(objectID, out bool wasOpened) && wasOpened)
        {
            OpenChest(true); // сразу открытый
        }
    }

    public void Interact() // вызов при нажатии игроком
    {
        if (!isOpen)
        {
            OpenChest(false);
            openedChests[objectID] = true; // сохраняем состояние
        }
    }

    private void OpenChest(bool instant)
    {
        isOpen = true;

        if (animator != null)
        {
            if (instant)
            {
                animator.SetTrigger("OpenChest"); // сразу в конце анимации
            }
            else
            {
                animator.SetTrigger("Open");
            }
        }

        if (chestCollider != null)
        {
            chestCollider.enabled = false; // отключаем коллайдер, если нужно
        }
    }
}