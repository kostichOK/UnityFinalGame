using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint; // точка спавна в этой сцене
    public Transform spawnPointEnemy;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            player.transform.localScale = spawnPoint.localScale;
        }

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null && spawnPoint != null)
        {
            enemy.transform.localScale = spawnPointEnemy.localScale;
        }
    }
}
