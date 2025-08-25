using UnityEngine;

public class DontDestroyEnemy : MonoBehaviour
{
    private static DontDestroyEnemy instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
