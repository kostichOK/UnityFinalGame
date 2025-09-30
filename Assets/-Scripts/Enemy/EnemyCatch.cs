using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyCatch : MonoBehaviour
{
    [Header("Animation / UI")]
    public Animator enemyAnimator;          // аниматор врага
    public string attackTrigger = "Attack"; // триггер анимации атаки
    public GameObject gameOverUI;           // экран проигрыша (в сцене или префаб)

    [Header("Capture behaviour")]
    public float captureDistance = 1.2f;        // дистанция в переди игрока, куда встанет враг
    public float facePlayerSpeed = 6f;          // скорость поворота врага и камеры
    public float gameOverDelay = 1.2f;          // задержка перед показом экрана
    public bool warpEnemyToCapturePoint = true; // будет ли враг "телепортироваться" в точку захвата
    public Transform capturePointOnPlayer;      // если указан — использовать эту Точку на игроке

    [Header("References (assign or auto-find)")]
    public NavMeshAgent agent;          // можно назначить вручную, иначе найдёт автоматически
    public Rigidbody rb;                // Rigidbody врага
    public Collider enemyCollider;      // Collider врага (обычно не trigger)
    [Tooltip("Добавь сюда скрипты игрока, которые нужно отключить (например: PlayerMovement, PlayerLook)")]
    public MonoBehaviour[] playerComponentsToDisable;

    // внутренние
    bool hasCaught = false;
    Transform player;
    Camera playerCam;
    MonoBehaviour[] cachedPlayerComponents; // чтобы можно было восстановить при нужде

    public AudioSource audioSource;
    public AudioSource audioSource1;

    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (enemyCollider == null) enemyCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCaught) return;

        if (other.CompareTag("Player"))
        {
            hasCaught = true;
            player = other.transform;
            playerCam = Camera.main;

            // Остановим NavMeshAgent / AI
            if (agent != null)
            {
                agent.ResetPath();
                agent.isStopped = true;
                agent.enabled = false; // отключаем, чтобы агент не "тянул" объект
            }

            // Отключаем физическое влияние
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Отключаем скрипты игрока (движение/обзор). Сохраним текущие для восстановления.
            cachedPlayerComponents = playerComponentsToDisable;
            foreach (var c in playerComponentsToDisable)
            {
                if (c != null) c.enabled = false;
            }

            // Перемещаем/выравниваем врага в точку захвата (чтобы анимация была видна и не проходила через игрока)
            if (warpEnemyToCapturePoint)
            {
                Vector3 target = ComputeCapturePosition();
                // Попробуем выбрать ближайшую точку на NavMesh (если есть), иначе просто ставим
                if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                }
                else
                {
                    transform.position = target;
                }
            }

            // Поворачиваем врага к игроку сразу (будет ещё плавно в Update)
            FacePlayerImmediate();

            // Запускаем корутину поворота камеры к врагу и атаки
            StartCoroutine(CaptureSequence());
        }
    }

    // Быстрый поворот врага лицом к игроку (без плавности)
    private void FacePlayerImmediate()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position);
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir.normalized);
    }
    // Корутин: поворачиваем камеру игрока к врагу (плавно), даём сыграть анимацию и показываем экран
    private IEnumerator CaptureSequence()
    {
        audioSource.Play();
        // Плавно повернуть камеру к врагу
        if (playerCam != null)
        {
            // Если камера управляется скриптом, он уже отключён выше (если ты добавил playerLook в список)
            Quaternion startRot = playerCam.transform.rotation;
            Vector3 lookTarget = transform.position;
            lookTarget.y = playerCam.transform.position.y; // ровняем по высоте камеры, чтобы не сильно клевало вниз/вверх
            Quaternion targetRot = Quaternion.LookRotation(lookTarget - playerCam.transform.position);

            float t = 0f;
            float dur = 0.4f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime * facePlayerSpeed;
                playerCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, Mathf.Clamp01(t / dur));
                yield return null;
            }

            // Зафиксировать точное направление
            playerCam.transform.rotation = targetRot;
        }

        // Повернём врага ещё точнее к игроку
        FacePlayerImmediate();

        // Запускаем анимацию атаки (если есть)
        if (enemyAnimator != null)
            enemyAnimator.SetTrigger(attackTrigger);

        // Ждём пока проиграется анимация (или задержка)
        yield return new WaitForSecondsRealtime(gameOverDelay);

        audioSource1.Stop();
        SceneManager.LoadScene(3);

        // Останавливаем время (опционально)
        Time.timeScale = 0f;
    }

    // Вычисляем желаемую позицию для вражеской "захватывающей" точки
    private Vector3 ComputeCapturePosition()
    {
        if (capturePointOnPlayer != null)
            return capturePointOnPlayer.position;

        // перед игроком на captureDistance (высота - та же, что у игрока)
        Vector3 forward = player.forward;
        Vector3 basePos = player.position + forward.normalized * captureDistance;

        // скорректируем высоту (оставим текущую высоту врага)
        basePos.y = transform.position.y;
        return basePos;
    }

    private void Update()
    {
        // Если поймал — плавно поворачиваем врага к игроку (визуально лучше)
        if (hasCaught && player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(direction.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * facePlayerSpeed);
            }
        }
    }

    // (опционально) метод для восстановления состояния (если хочешь перезапустить уровень без перезапуска сцены)
    public void RestoreAfterCatch()
    {
        // восстановим компоненты игрока
        if (cachedPlayerComponents != null)
        {
            foreach (var c in cachedPlayerComponents)
            {
                if (c != null) c.enabled = true;
            }
        }

        // включим NavMeshAgent и физику (если нужно)
        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        hasCaught = false;

        // убрать gameOverUI если надо
        if (gameOverUI != null) gameOverUI.SetActive(false);

        Time.timeScale = 1f;
    }
}