using UnityEngine;

public class DDNicklakl : MonoBehaviour
{
    private static DDNicklakl instance;

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