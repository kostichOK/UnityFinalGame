using UnityEngine;

public class FinalSequence : MonoBehaviour
{
    public Transform player; // Игрок
    public GameObject artifactLeftPrefab;
    public GameObject artifactCenterPrefab;
    public GameObject artifactRightPrefab;
    public Transform spawnPointLeft;
    public Transform spawnPointCenter;
    public Transform spawnPointRight;
    public GameObject finalLight; // Сильный свет для финала
    public float displayTime = 3f; // Время перед включением света

    private GameObject leftArtifact;
    private GameObject centerArtifact;
    private GameObject rightArtifact;

    void Start()
    {
        StartCoroutine(PlayFinalSequence());
    }

    private System.Collections.IEnumerator PlayFinalSequence()
    {
        // Спавним артефакты
        leftArtifact = Instantiate(artifactLeftPrefab, spawnPointLeft.position, spawnPointLeft.rotation);
        centerArtifact = Instantiate(artifactCenterPrefab, spawnPointCenter.position, spawnPointCenter.rotation);
        rightArtifact = Instantiate(artifactRightPrefab, spawnPointRight.position, spawnPointRight.rotation);

        // Пусть смотрят на игрока
        leftArtifact.transform.LookAt(player);
        centerArtifact.transform.LookAt(player);
        rightArtifact.transform.LookAt(player);

        // Ждем немного, чтобы игрок увидел артефакты
        yield return new WaitForSeconds(displayTime);

        // Включаем финальный свет
        finalLight.SetActive(true);

        // Можно убрать артефакты, если не нужны после финала
        Destroy(leftArtifact);
        Destroy(centerArtifact);
        Destroy(rightArtifact);
    }
}
