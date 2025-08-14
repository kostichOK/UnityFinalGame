using UnityEngine;

public class PlayerCatch : MonoBehaviour
{
    public float pickupDistance = 3f;
    public LayerMask itemLayer;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); // Центр экрана
        RaycastHit hit;

        // Рисуем луч зелёным (до точки соприкосновения, если есть)
        if (Physics.Raycast(ray, out hit, pickupDistance, itemLayer))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green); // до объекта
            Debug.Log("Можно подобрать: " + hit.collider.name);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Предмет подобран: " + hit.collider.name);
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            // Если никуда не попал — рисуем на полную длину
            Debug.DrawRay(ray.origin, ray.direction * pickupDistance, Color.red);
        }
    }
}
