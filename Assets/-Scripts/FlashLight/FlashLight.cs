using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject GameObject;
    bool isTurnedOn = false;
    public AudioSource audioSource;

    private void Update()
    {
        TurnOnFlashlight();
    }
    public void TurnOnFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isTurnedOn)
            {
                isTurnedOn = true;
                GameObject.SetActive(true);
                audioSource.Play();
            }
            else
            {
                isTurnedOn = false;
                GameObject.SetActive(false);
                audioSource.Play();
            }
        }
    }
}
