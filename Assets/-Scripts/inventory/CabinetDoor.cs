using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    public Animator animator;
    public LayerMask interactLayer;
    private bool isOpen = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        if (animator == null)
        {
            Debug.Log("FFFFF");
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 3f, interactLayer))
        {
            // Проверяем, что луч попал именно в этот объект
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log($"Попал в {gameObject.name}");

                if (Input.GetMouseButtonDown(0))
                {
                    OpenDoor();
                }
            }
        }
    }

    private void OpenDoor()
    {
        Debug.Log("z");
        isOpen = true;
        animator.SetBool("isOpen", true);
    }
}