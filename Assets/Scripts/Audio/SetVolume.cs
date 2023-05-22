using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    private void Start()
    {
        mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        float volume;
        mixer.GetFloat("MasterVol", out volume);
        slider.value = volume;
    }

    public void SetMasterVolume  (float sliderValue)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVol", sliderValue);
    }

    private void update ()
    {
        mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        float volume;
        mixer.GetFloat("MusicVol", out volume);
        slider.value = volume;
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
    }

}
