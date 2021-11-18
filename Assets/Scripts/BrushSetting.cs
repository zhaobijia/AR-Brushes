using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]

public class BrushSetting : MonoBehaviour
{

    [SerializeField] Draw draw;

    [Header("Default Line Renderer Setting:")]
    //default settings:
    public float defaultStartWidth;
    public float defaultEndWidth;
    public Color defaultStartColor;
    public Color defaultEndColor;
    [Space(10)]
    public Material defaultMaterial;
    public bool defaultUseWorldSpace;
    
    [Space(10)]

    [Header("Line Renderer Setting UI:")]
    [SerializeField] Slider distanceSlider;
    
    [SerializeField] float minDistance = 0.2f;
    [SerializeField] float maxDistance = 1.5f;

    [SerializeField] Slider brushSizeSlider;
    [SerializeField] float minSize = 0.01f;
    [SerializeField] float maxSize = 1f;

    [SerializeField] Image currentColorIndicator;
    [SerializeField] Image colorPickerImage;
    [SerializeField] ColorPicker colorPicker;

    private void Start()
    {
        if (distanceSlider != null)
        {
            distanceSlider.onValueChanged.AddListener(delegate { ChangeDrawDistance(); });
        }

        if(brushSizeSlider != null)
        {
            brushSizeSlider.onValueChanged.AddListener(delegate { ChangeBrushSize(); });
        }

        if(currentColorIndicator != null)
        {
            colorPicker.onColorChanged.AddListener(delegate { UpdateBrushColor(); });
        }

        
    }

   
    public void DefaultLineSetting(CustomLineRenderer currentLine)
    {

        //apply default line setting
        currentLine.startRadius = defaultStartWidth;
        currentLine.endRadius = defaultEndWidth;
        currentLine.startColor = defaultStartColor;
        currentLine.endColor = defaultEndColor;

        currentLine.material = defaultMaterial;
        currentLine.useWorldSpace = defaultUseWorldSpace;

    }

    

    #region Spawn Distance from Camera
    private void ChangeDrawDistance()
    {
        float sliderValue = distanceSlider.value;
        
        //distance range from 0.2 to 1.5
        float dist = minDistance + (maxDistance - minDistance) * sliderValue;

        draw.SetDistanceFromCamera(dist);
    }
    #endregion

    #region Brush Size
    private void ChangeBrushSize()
    {
        float sliderValue = brushSizeSlider.value;
        float size = minSize + (maxSize - minSize) * sliderValue;
        defaultStartWidth = size;
        defaultEndWidth = size;

    }
    #endregion

    #region Brush Color
    public void ToggleColorPicker()
    {
        if (colorPickerImage.enabled)
        {
            colorPickerImage.enabled = false;
        }
        else
        {
            colorPickerImage.enabled = true;
        }
    }

    
    private void UpdateBrushColor()
    {
        currentColorIndicator.color = colorPicker.pickedColor;
        defaultStartColor = colorPicker.pickedColor;
        defaultEndColor = colorPicker.pickedColor;
    }
    #endregion

    #region
    public void ToggleBrushSet()
    {

    }

    #endregion

}
