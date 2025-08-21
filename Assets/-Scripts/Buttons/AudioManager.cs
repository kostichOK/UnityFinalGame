using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;   
    [SerializeField] private AudioMixer audioMixer; 
    private const string VolumeKey = "Volume"; 
    private const string MixerParameter = "MasterVolume"; 
    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.75f);

        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
    private void SetVolume(float value)
    {
        PlayerPrefs.SetFloat(VolumeKey, value);
        float volumeInDb = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(MixerParameter, volumeInDb);
    }
}
