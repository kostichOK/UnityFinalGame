using UnityEngine;

public class Mute : MonoBehaviour
{
    public bool muteOnStart = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (muteOnStart)
            AudioListener.volume = 0f; // полностью глушим звук
    }
}
