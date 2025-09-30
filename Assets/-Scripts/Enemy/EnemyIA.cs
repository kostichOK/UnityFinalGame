using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public PointsManager pointsManager;
    private int currentPointIndex = 0;

    private NavMeshAgent agent;

    public Transform player;
    public float visionRange = 10f;
    public int rayCount = 10;
    public float visionAngle = 120f;
    public Vector3 rayOriginOffset = new Vector3(0, 1.0f, 0);

    private bool playerSeen = false;
    private float lostPlayerTimer = 0f;
    public float lostPlayerCooldown = 10f; // сколько секунд ждать перед возвратом к патрулю

    public AudioSource audioSource;
    public float fadeOutDuration = 2f; // время затухания музыки
    private Coroutine fadeCoroutine;

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

            // Включаем музыку погони, если она ещё не играет
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 1f;
                audioSource.Play();

                // Если шло затухание, останавливаем его
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                    fadeCoroutine = null;
                }
            }
        }
        else
        {
            lostPlayerTimer += Time.deltaTime;

            if (lostPlayerTimer >= lostPlayerCooldown)
            {
                Patrol();

                // Запускаем плавное затухание музыки, если она играет
                if (audioSource.isPlaying && fadeCoroutine == null)
                {
                    fadeCoroutine = StartCoroutine(FadeOutMusic());
                }
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

    private IEnumerator FadeOutMusic()
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 1f;
        fadeCoroutine = null;
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