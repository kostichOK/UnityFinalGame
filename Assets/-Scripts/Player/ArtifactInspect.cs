using System.Collections;
using UnityEngine;

public class ArtifactInspection : MonoBehaviour
{
    public Transform inspectPoint;
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public Camera playerCam;
    public LayerMask interactLayer;
    public GameObject postProcessVolume;
    public MonoBehaviour playerMovement;
    public MonoBehaviour playerLook;
    public GameObject pointLight;
    public GameObject cursorNormal;
    public GameObject cursorSee;

    private Transform currentItem;
    private GameObject currentArtifact;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool inspecting = false;
    private bool itemReady = false;

    void Update()
    {
        if (!inspecting)
        {
            HandleRaycast();
        }
        else
        {
            RotateItem();
            CheckPickup(); // Нажатие E → выключение предмета
        }
    }

    private void HandleRaycast()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 5f, interactLayer))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform != null && !inspecting)
                {
                    StartCoroutine(StartInspect(hit.transform));
                }
            }
        }
    }

    private void RotateItem()
    {
        if (currentItem != null)
        {
            float rotX = -Input.GetAxis("Mouse X") * rotateSpeed * Time.unscaledDeltaTime;
            float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.unscaledDeltaTime;

            currentItem.Rotate(playerCam.transform.up, rotX, Space.World);
            currentItem.Rotate(playerCam.transform.right, rotY, Space.World);
        }
    }

    private void CheckPickup()
    {
        // Нажатие E → "собираем" артефакт (просто скрываем)
        if (Input.GetKeyDown(KeyCode.E) && itemReady)
        {
            if (currentArtifact != null)
            {
                currentArtifact.SetActive(false); // как будто положили в рюкзак
            }
            ResetInspection();
        }
    }

    private IEnumerator StartInspect(Transform itemTransform)
    {
        inspecting = true;
        currentArtifact = itemTransform.gameObject;
        currentItem = itemTransform;
        originalPos = itemTransform.position;
        originalRot = itemTransform.rotation;

        // Отключаем физику на время осмотра
        if (itemTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        Time.timeScale = 0f;
        postProcessVolume.SetActive(true);
        pointLight.SetActive(true);
        playerMovement.enabled = false;
        playerLook.enabled = false;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * moveSpeed;
            currentItem.position = Vector3.Lerp(originalPos, inspectPoint.position, t);
            currentItem.rotation = Quaternion.Slerp(originalRot, inspectPoint.rotation, t);
            yield return null;
        }

        itemReady = true;
    }

    private void ResetInspection()
    {
        itemReady = false;
        Time.timeScale = 1f;
        postProcessVolume.SetActive(false);
        pointLight.SetActive(false);
        playerMovement.enabled = true;
        playerLook.enabled = true;
        inspecting = false;
        currentItem = null;
        currentArtifact = null;
    }
}