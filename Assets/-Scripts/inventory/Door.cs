using System;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public Camera playerCam;
    public GameObject cursorNormal;
    public GameObject cursorSee;
    public LayerMask interactLayer;
    private bool isOpen = false;

    private void Start()
    {
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (camObj != null)
        {
            playerCam = camObj.GetComponent<Camera>();
        }
        else
        {
            Debug.LogWarning("Камера не найдена!");
        }

        cursorNormal = GameObject.FindGameObjectWithTag("CN");
        cursorSee = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(o => o.CompareTag("CS"));

        // попытаемся автоматически найти Animator, если не привязан
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        ComprobarDoor();
    }

    public void Open()
    {
        Debug.Log("a");
        isOpen = true;
        animator.SetBool("isOpen", true);
    }

    public void Close()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
        Debug.Log("b");
    }

    private void ComprobarDoor()
    {

        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        cursorNormal.SetActive(true);
        cursorSee.SetActive(false);

        if (Physics.Raycast(ray, out RaycastHit hit, ButtonsManager.rayLarge, interactLayer))
        {
            cursorNormal.SetActive(false);
            cursorSee.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform != null)
                {
                    if (isOpen == true)
                    {
                        Close();
                    }
                    else
                    {
                        Open();
                    }
                }
            }
        }
    }

    private Camera FindGameObjectWithTag(string v)
    {
        throw new NotImplementedException();
    }
}