using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;  
    public Slider volumeSlider;    

    private const string VolumeKey = "MasterVolume";

    void Start()
    {
       
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.75f); 
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

       
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
       
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }
}
