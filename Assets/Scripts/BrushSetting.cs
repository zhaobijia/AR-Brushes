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
    public float lineWidth = 0.5f;
    public Color lineColor = Color.white;
    [Space(10)]
    public Material lineMaterial;
  //  public bool defaultUseWorldSpace;
    
    [Space(10)]

    [Header("Line Renderer Setting UI:")]
    [SerializeField] Slider distanceSlider;
    
    [SerializeField] float minDistance = 0.1f;
    [SerializeField] float maxDistance = 10f;

    [SerializeField] Slider brushSizeSlider;
    [SerializeField] float minSize = 1f;
    [SerializeField] float maxSize = 10f;

    [SerializeField] Image currentColorIndicator;
    [SerializeField] Image colorPickerImage;
    [SerializeField] ColorPicker colorPicker;

    private void Start()
    {
        if (distanceSlider != null)
        {
            distanceSlider.onValueChanged.AddListener(delegate { ChangeDrawDistance(); });
            distanceSlider.maxValue = maxDistance;
            distanceSlider.minValue = minDistance;
        }

        if(brushSizeSlider != null)
        {
            brushSizeSlider.onValueChanged.AddListener(delegate { ChangeBrushSize(); });
            brushSizeSlider.maxValue = maxSize;
            brushSizeSlider.minValue = minSize;
        }

        if(currentColorIndicator != null)
        {
            colorPicker.onColorChanged.AddListener(delegate { UpdateBrushColor(); });
        }

        
    }

   
    public void LineSetting(CustomLineRenderer currentLine)
    {

        //apply default line setting
        currentLine.radius = lineWidth;
        currentLine.color = lineColor;
        currentLine.material = lineMaterial;

    }

    

    #region Spawn Distance from Camera
    private void ChangeDrawDistance()
    {

        //distance range from 0.2 to 1.5
        float dist = distanceSlider.value;

        draw.SetDistanceFromCamera(dist);
    }
    #endregion

    #region Brush Size
    private void ChangeBrushSize()
    {
        lineWidth = brushSizeSlider.value;
 
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
        lineColor = colorPicker.pickedColor;
        
    }
    #endregion

    #region
    public void ToggleBrushSet()
    {

    }

    #endregion

}
