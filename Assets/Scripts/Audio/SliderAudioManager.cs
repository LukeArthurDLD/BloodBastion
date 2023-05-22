using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class SliderAudioManager : MonoBehaviour
{
    public AudioMixer mainMixer;
    public string variable = "MasterVol";
    //public TextMeshProUGUI sliderNumber;
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        if (PlayerPrefs.HasKey(variable))
        {
            SetLevel(PlayerPrefs.GetFloat(variable));
            slider.value = PlayerPrefs.GetFloat(variable);
        }
    }

    void OnSliderChanged(float value)
    {
        SetLevel(value);
    }


    public void SetLevel (float sliderValue)
    {
        //sliderNumber.text = ((int)(sliderValue * 100)).ToString();
        mainMixer.SetFloat(variable, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(variable, sliderValue);
    }
}
