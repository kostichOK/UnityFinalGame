using System;
using System.Collections;
using UnityEngine;

public class ItemInspection : MonoBehaviour
{
    public Transform inspectPoint;
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public Camera playerCam;
    public LayerMask interactLayer;
    public LayerMask interactLayer2;
    public GameObject postProcessVolume;
    public MonoBehaviour playerMovement;
    public MonoBehaviour playerLook;

    private Transform currentItem;
    public GameObject currentItems;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool inspecting;
    public GameObject pointLight;
    public GameObject cursorNormal;
    public GameObject cursorSee;
    public bool itemReady = false;
    public HoldItem holdItem;
    public bool handOcuped = false;

    public Item item;
    public ButtonsManager buttonsManager;

    [Obsolete]
    void Update()
    {
        if (!inspecting)
        {
            HandleRaycast();
        }
        else
        {
            RotateItem();
            CheckEndInspect();
            CheckPickup();
        }
    }

    [Obsolete]
    private void HandleRaycast()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        cursorNormal.SetActive(true);
        cursorSee.SetActive(false);

        if (Physics.Raycast(ray, out RaycastHit hit, ButtonsManager.rayLarge, interactLayer))
        {
            cursorNormal.SetActive(false);
            cursorSee.SetActive(true);

            if (Physics.Raycast(ray, out RaycastHit hit2, ButtonsManager.rayLarge, interactLayer2))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.transform != null && handOcuped == false)
                    {
                        StartCoroutine(StartInspect(hit.transform));
                    }
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

    [Obsolete]
    private void CheckEndInspect()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(EndInspect());
        }
    }

    [Obsolete]
    private void CheckPickup()
    {
        if (Input.GetKeyDown(KeyCode.E) && itemReady && handOcuped == false)
        {
            if (currentItems != null)
            {
                var itemComponent = currentItem.GetComponent<Item>();
                holdItem.Hold(currentItems);
                handOcuped = true;
                Debug.Log(handOcuped);
                itemComponent.PickUp();
            }

            ResetInspection();
        }
    }

    [Obsolete]
    public IEnumerator StartInspect(Transform itemTransform)
    {
        currentItems = itemTransform.gameObject;
        cursorSee.SetActive(false);
        inspecting = true;

        currentItem = itemTransform;
        originalPos = itemTransform.position;
        originalRot = itemTransform.rotation;

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

    [Obsolete]
    public IEnumerator EndInspect()
    {
        itemReady = false;

        float t = 0f;
        Vector3 startPos = currentItem.position;
        Quaternion startRot = currentItem.rotation;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * moveSpeed;
            currentItem.position = Vector3.Lerp(startPos, originalPos, t);
            currentItem.rotation = Quaternion.Slerp(startRot, originalRot, t);
            yield return null;
        }

        if (currentItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        Time.timeScale = 1f;
        postProcessVolume.SetActive(false);
        pointLight.SetActive(false);
        playerMovement.enabled = true;
        playerLook.enabled = true;

        inspecting = false;
        currentItem = null;
        currentItems = null;
    }

    public void ResetInspection()
    {
        itemReady = false;
        pointLight.SetActive(false);
        Time.timeScale = 1f;
        postProcessVolume.SetActive(false);
        playerMovement.enabled = true;
        playerLook.enabled = true;
        inspecting = false;
        currentItem = null;
        currentItems = null;

        // handOcuped сбрасываем только если предмет не был поднят
        if (!holdItem.HasItem())
        {
            handOcuped = false;
        }
    }
}