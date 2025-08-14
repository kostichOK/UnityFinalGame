using Unity.VisualScripting;
using UnityEngine;

public class ItemInspection : MonoBehaviour
{
    public Transform inspectPoint;
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public Camera playerCam;
    public LayerMask interactLayer;
    public GameObject postProcessVolume; 
    public MonoBehaviour playerMovement; 
    public MonoBehaviour playerLook;     

    private Transform currentItem;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool inspecting;
    public GameObject pointLight;

    void Update()
    {
        if (!inspecting)
        {
            Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 3f, interactLayer))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {

                    StartCoroutine(StartInspect(hit.transform));
                }
            }
        }
        else
        {
            // Вращение предмета мышкой
            float rotX = -Input.GetAxis("Mouse X") * rotateSpeed * Time.unscaledDeltaTime;
            float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.unscaledDeltaTime;
            currentItem.Rotate(playerCam.transform.up, rotX, Space.World);
            currentItem.Rotate(playerCam.transform.right, rotY, Space.World);

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StartCoroutine(EndInspect());
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(currentItem.gameObject);
                pointLight.SetActive(false);

                Time.timeScale = 1f;
                postProcessVolume.SetActive(false);
                playerMovement.enabled = true;
                playerLook.enabled = true;

                inspecting = false;
                currentItem = null;
            }
        }
    }

    System.Collections.IEnumerator StartInspect(Transform item)
    {
        inspecting = true;
        currentItem = item;
        originalPos = item.position;
        originalRot = item.rotation;

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = true;

        Time.timeScale = 0f;
        postProcessVolume.SetActive(true);
        pointLight.SetActive(true);
        playerMovement.enabled = false;
        playerLook.enabled = false;

        // Плавная анимация
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * moveSpeed;
            currentItem.position = Vector3.Lerp(originalPos, inspectPoint.position, t);
            currentItem.rotation = Quaternion.Slerp(originalRot, inspectPoint.rotation, t);
            yield return null;
        }
    }

    System.Collections.IEnumerator EndInspect()
    {
        float t = 0;
        Vector3 startPos = currentItem.position;
        Quaternion startRot = currentItem.rotation;

        // Плавная анимация
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * moveSpeed;
            currentItem.position = Vector3.Lerp(startPos, originalPos, t);
            currentItem.rotation = Quaternion.Slerp(startRot, originalRot, t);
            yield return null;
        }

        if (currentItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = false;

        Time.timeScale = 1f;
        postProcessVolume.SetActive(false);
        pointLight.SetActive(false);
        playerMovement.enabled = true;
        playerLook.enabled = true;

        inspecting = false;
        currentItem = null;
    }
}