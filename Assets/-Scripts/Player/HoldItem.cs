using System.Collections;
using UnityEngine;

public class HoldItem : MonoBehaviour
{
    public Transform holdPoint; // точка на руке
    private GameObject currentItem; // предмет, который держим
    public ItemInspection inspection;

    private static HoldItem instance;
    public Item item;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Obsolete]
    void Update()
    {
        // выбросить предмет по Q
        if (Input.GetKeyDown(KeyCode.G) && currentItem != null)
        {
            Drop();
        }
    }

    // Вызываем этот метод, когда подбираем новый предмет
    [System.Obsolete]
    public void Hold(GameObject newItem)
    {
        if (inspection.handOcuped)
        {
            inspection.EndInspect();
            inspection.ResetInspection();
            return;
        }

        currentItem = newItem;

        // фиксируем у руки
        currentItem.transform.SetParent(holdPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.Euler(-193, 4, 192);

        if (currentItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // полностью отключаем физику, чтобы НЕ мешала
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.detectCollisions = false;

        }
    }

    // Чтобы бросить или убрать предмет
    public void Release()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);
        }
        currentItem = null;
        inspection.currentItems = null;
        inspection.handOcuped = false;
        inspection.itemReady = false;
    }

    [System.Obsolete]
    public void Drop(float? force = null)
    {
        float dropPower = force ?? FindObjectOfType<ButtonsManager>().dropForce;

        if (currentItem != null)
        {
            var rb = currentItem.GetComponent<Rigidbody>();
            currentItem.transform.SetParent(null);

            if (rb != null)
            {
                inspection.handOcuped = false;
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.detectCollisions = true;
                rb.AddForce(Camera.main.transform.forward * dropPower, ForceMode.Impulse);

                StartCoroutine(SavePositionWhenStopped(currentItem, rb));
            }

            currentItem = null;
            inspection.currentItems = null;
            inspection.handOcuped = false;
            inspection.itemReady = false;
        }
    }

    [System.Obsolete]
    private IEnumerator SavePositionWhenStopped(GameObject droppedItem, Rigidbody rb)
    {
        // Ждем, пока объект почти не движется
        while (rb.velocity.magnitude > 0.05f || rb.angularVelocity.magnitude > 0.05f)
        {
            yield return null;
        }

        Vector3 newPos = droppedItem.transform.position;
        Debug.Log("Item stopped at: " + newPos);

        // Тут можно вызывать твою логику сохранения позиции
        var itemScript = droppedItem.GetComponent<Item>();
        if (itemScript != null)
        {
            itemScript.Drop(newPos); // передаём позицию в Item
        }
    }

    public GameObject GetCurrentItem()
    {
        return currentItem;
    }
}