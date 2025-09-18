using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] ButtonsSO buttonSO;
    public GameObject mapBigImage;
    public GameObject mapSmallImage;
    bool mapIsActive = false;

    public Light flashlight;
    [SerializeField] private float range;
    [SerializeField] private float intensity;

    public PostProcessVolume volume;
    [SerializeField] DepthOfField dof;
    [SerializeField] private float focusDistance;
    [SerializeField] private float aperture;

    public Transform spawnPoint; // точка спавна игрока в новой сцене
    private NavMeshAgent agent;
    public PlayerMovement playerSpeed;
    public EnemyAI enemyAI;
    public HoldItem holdItem;
    public PointsManager pointsManager;
    public GameObject cursorSee;
    public static float rayLarge = 0.8f;
    public float dropForce = 3;
    int currentScene = 0;

    private void Start()
    {
        // Получаем DOF из Volume, если ещё не назначен
        if (volume != null && dof == null)
        {
            if (volume.profile.TryGetSettings<DepthOfField>(out var depth))
                dof = depth;
        }
    }

    private void Update()
    {
        // Управление картой
        if (buttonSO.mapActive && Input.GetKeyDown(KeyCode.Q))
        {
            mapIsActive = !mapIsActive;
            if (mapBigImage == null)
            {
                mapBigImage = GameObject.Find("MapImage"); // автоматически ищем, если не назначено
            }
            if (mapBigImage) mapBigImage.SetActive(mapIsActive);

            Cursor.lockState = mapIsActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = mapIsActive;
        }

        if (buttonSO.closeMap && mapBigImage)
        {
            mapBigImage.SetActive(false);
            mapIsActive = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    [System.Obsolete]
    public void ToIndustry()
    {
        if (currentScene == 1) return;
        currentScene = 1;
        buttonSO.mapActive = false;
        if (mapBigImage) mapBigImage.SetActive(false);
        mapIsActive = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Настраиваем фонарик и пост-процесс
        if (flashlight)
        {
            flashlight.intensity = 1.8f;
            flashlight.range = 9.5f;
        }
        if (dof != null)
        {
            dof.focusDistance.value = 1.28f;
            dof.aperture.value = 32;
            dof.focalLength.value = 183;
        }

        mapSmallImage.SetActive(false);
        GameObject enemy = GameObject.FindWithTag("Enemy");
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.speed = 2;
        playerSpeed.walkSpeedRef = 3;
        playerSpeed.walkSpeed = 3;
        playerSpeed.lookSpeed = 2f;

        enemyAI.visionRange = 26;
        rayLarge = 3f;
        dropForce = 5;

        // Подписываемся на событие загрузки новой сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1);
    }

    [System.Obsolete]
    public void ToSementery()
    {
        if (currentScene == 2) return;
        currentScene = 2;

        if (mapBigImage) mapBigImage.SetActive(false);
        mapIsActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        mapSmallImage.SetActive(false);

        agent.speed = 1;
        playerSpeed.walkSpeedRef = 1.5f;
        playerSpeed.walkSpeed = 1.5f;
        playerSpeed.lookSpeed = 2f;

        if (flashlight)
        {
            flashlight.intensity = 0.67f;
            flashlight.range = 4f;
        }
        if (dof != null)
        {
            dof.focusDistance.value = 0.3f;
            dof.aperture.value = 19.8f;
            dof.focalLength.value = 46;
        }

        enemyAI.visionRange = 13;
        rayLarge = 0.8f;
        dropForce = 3;


        SceneManager.LoadScene(0);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [System.Obsolete]
    public void ToShed()
    {
        if (currentScene == 3) return;
        currentScene = 3;

        cursorSee.SetActive(true);
        if (mapBigImage) mapBigImage.SetActive(false);
        mapIsActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        mapSmallImage.SetActive(false);

        GameObject enemy = GameObject.FindWithTag("Enemy");
        agent = enemy.GetComponent<NavMeshAgent>();

        agent.speed = 3;
        playerSpeed.walkSpeedRef = 5f;
        playerSpeed.walkSpeed = 5f;
        playerSpeed.lookSpeed = 3f;

        if (flashlight)
        {
            flashlight.intensity = 2f;
            flashlight.range = 30f;
        }

        enemyAI.visionRange = 30;
        rayLarge = 8f;
        dropForce = 10;

        SceneManager.LoadScene(2);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [System.Obsolete]
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Игрок
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && spawnPoint != null)
        {
            player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        }

        // Враг
        GameObject enemy = GameObject.FindWithTag("Enemy");
        GameObject espGO = GameObject.FindWithTag("EnemySpawn"); // точка спавна врага

        if (enemy != null && espGO != null)
        {
            Transform esp = espGO.transform;
            var agent = enemy.GetComponent<NavMeshAgent>();
            var rb = enemy.GetComponent<Rigidbody>();

            if (agent) agent.enabled = false;
            if (rb)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            enemy.transform.SetPositionAndRotation(esp.position, esp.rotation);

            if (agent)
            {
                agent.Warp(esp.position);
                agent.enabled = true;
            }

            if (rb) rb.isKinematic = false;

            Debug.Log($"[Spawn] Enemy -> {esp.position}");
        }
        else
        {
            Debug.LogWarning($"[Spawn] Enemy or EnemySpawn missing. enemy:{enemy != null} spawn:{espGO != null}");
        }

        // Автоматическое присвоение mapImage, если оно пропало
        if (mapSmallImage == null)
        {
            mapSmallImage = GameObject.Find("MapImage");
        }

        // Находим все ExitZone на сцене
        ExitZone[] zones = FindObjectsOfType<ExitZone>();
        foreach (var zone in zones)
        {
            zone.SetMapButton(mapSmallImage); // передаем ссылку на кнопку
        }
    }
}