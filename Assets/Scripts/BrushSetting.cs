using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]

public class BrushSetting : MonoBehaviour
{
    public bool settingIsOn;
    [SerializeField] Draw draw;

    [Header("Default Line Renderer Setting:")]
    //default settings:
    public float lineWidth;
    public Color lineColor;
    [Space(10)]
    public Material lineMaterial;
  //  public bool defaultUseWorldSpace;
    
    [Space(10)]

    [Header("Line Renderer Setting UI:")]
    [SerializeField] Slider distanceSlider;
    
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    [SerializeField] Slider brushSizeSlider;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    [SerializeField] Image currentColorIndicator;
    [SerializeField] Image colorPickerImage;
    [SerializeField] ColorPicker colorPicker;

    [SerializeField] GameObject brushSetGO;
    bool brushPanelIsOn;

    private void Start()
    {
        if (distanceSlider != null)
        {
            distanceSlider.maxValue = maxDistance;
            distanceSlider.minValue = minDistance;
            distanceSlider.onValueChanged.AddListener(delegate { ChangeDrawDistance(); });
            
        }

        if(brushSizeSlider != null)
        {
            brushSizeSlider.maxValue = maxSize;
            brushSizeSlider.minValue = minSize;
            brushSizeSlider.onValueChanged.AddListener(delegate { ChangeBrushSize(); });
            
        }

        if(currentColorIndicator != null)
        {
            colorPicker.onColorChanged.AddListener(delegate { UpdateBrushColor(); });
        }

        
    }

   
    public void LineSetting(CustomLineRenderer currentLine)
    {

        currentLine.radius = lineWidth;
        currentLine.color = lineColor;
        currentLine.material = lineMaterial;

    }

    

    #region Spawn Distance from Camera
    private void ChangeDrawDistance()
    {
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
            settingIsOn = false;
        }
        else
        {
            colorPickerImage.enabled = true;
            settingIsOn = true;
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
        if (brushPanelIsOn)
        {
            brushSetGO.SetActive(false);
            brushPanelIsOn = false;
        }
        else
        {
            brushSetGO.SetActive(true);
            brushPanelIsOn = true;
        }
    }

    #endregion

}
