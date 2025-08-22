using UnityEngine;

public class HoldItem : MonoBehaviour
{
    public Transform holdPoint; // точка на руке
    private GameObject currentItem; // предмет, который держим
    public ItemInspection inspection;

    void Update()
    {
        // выбросить предмет по Q
        if (Input.GetKeyDown(KeyCode.Q) && currentItem != null)
        {
            Drop();
        }
    }

    // Вызываем этот метод, когда подбираем новый предмет
    public void Hold(GameObject newItem)
    {
        if (inspection.handOcuped)
        {
            inspection.EndInspect();
            inspection.ResetInspection();
            Debug.Log("gg");
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

            // очень важно — обнуляем скорость
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
        inspection.handOcuped = false; 
    }

    public void Drop(float force = 5f)
    {
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


                rb.AddForce(Camera.main.transform.forward * force, ForceMode.Impulse);
            }

            currentItem = null;
        }
    }

    public GameObject GetCurrentItem()
    {
        return currentItem;
    }
}