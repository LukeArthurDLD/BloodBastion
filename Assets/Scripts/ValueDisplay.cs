using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueDisplay : MonoBehaviour
{
    //technical
    private Slider slider;
    public TextMeshProUGUI text;

    //colour
    public bool useGradient = true;
    public Gradient gradient;
    public Image fill;

    //camera
    public bool faceCamera = true;
    private Transform cam;
      
    private void Start()
    {
        // get components
        cam = Camera.main.transform;
        slider = GetComponent<Slider>();
        //if (slider.fillRect != null)
        //    fill = slider.fillRect.GetComponent<Image>();
    }
    public void SetMaxValue(float value)
    {
        if (slider)
        {
            slider.maxValue = value;
            slider.value = value;
            if(useGradient && fill)
                fill.color = gradient.Evaluate(1f);
        }
        if (text)
            text.text = value.ToString();

    }
    public void SetValue(float value)
    {
        if (slider)
        {
            slider.value = value;
            // for colour
            if (useGradient && fill)
                fill.color = gradient.Evaluate(slider.normalizedValue);
        }
        if (text)
        {
            text.text = value.ToString();
        }

    }
    private void LateUpdate()
    {
        if (faceCamera)
            transform.LookAt(transform.position + cam.forward);
    }
}
