using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Патруль")]
    public PointsManager pointsManager;
    private int currentPointIndex = 0;

    [Header("Навигация")]
    private NavMeshAgent agent;

    [Header("Игрок и зрение")]
    public Transform player;
    public float visionRange = 10f;
    public int rayCount = 10;
    public float visionAngle = 120f;
    public Vector3 rayOriginOffset = new Vector3(0, 1.0f, 0);

    private bool playerSeen = false;
    private float lostPlayerTimer = 0f;
    public float lostPlayerCooldown = 2f; // сколько секунд ждать перед возвратом к патрулю

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (pointsManager != null && pointsManager.positionsCount() > 0)
        {
            transform.position = pointsManager.GetPosition(0);
            agent.SetDestination(pointsManager.GetPosition(0));
        }
    }

    void Update()
    {
        DetectPlayer();

        if (playerSeen)
        {
            lostPlayerTimer = 0f;
            agent.SetDestination(player.position);
        }
        else
        {
            lostPlayerTimer += Time.deltaTime;

            if (lostPlayerTimer >= lostPlayerCooldown)
            {
                Patrol();
            }
        }
    }

    void Patrol()
    {
        if (pointsManager == null || pointsManager.positionsCount() == 0)
            return;

        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
        {
            currentPointIndex++;
            if (currentPointIndex >= pointsManager.positionsCount())
                currentPointIndex = 0;

            agent.SetDestination(pointsManager.GetPosition(currentPointIndex));
        }
    }

    void DetectPlayer()
    {
        playerSeen = false;
        Vector3 origin = transform.position + rayOriginOffset;

        float startAngle = -visionAngle / 2f;
        float angleStep = visionAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, visionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    playerSeen = true;
                    break;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position + rayOriginOffset;

        float startAngle = -visionAngle / 2f;
        float angleStep = visionAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, visionRange))
            {
                Gizmos.color = hit.collider.CompareTag("Player") ? Color.green : Color.red;
                Gizmos.DrawLine(origin, hit.point);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * visionRange);
            }
        }
    }
}
