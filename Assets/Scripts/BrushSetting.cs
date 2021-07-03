using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSetting : MonoBehaviour
{
    [SerializeField] Draw draw;

    [SerializeField] Slider distanceSlider;
    [SerializeField] float minDistance = 0.2f;
    [SerializeField] float maxDistance = 1.5f;

    private void Start()
    {
        if (distanceSlider != null)
        {
            distanceSlider.onValueChanged.AddListener(delegate { ChangeDrawDistance(); });
        }
    }
    private void ChangeDrawDistance()
    {
        float sliderValue = distanceSlider.value;
        
        //distance range from 0.2 to 1.5
        float dist = minDistance + (maxDistance - minDistance) * sliderValue;

        draw.SetDistanceFromCamera(dist);
    }
}
