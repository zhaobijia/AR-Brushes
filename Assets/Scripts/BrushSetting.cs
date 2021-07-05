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
    public int defaultCornerVertices;//3
    public int defaultEndCapVertices;//5
    public LineTextureMode defaultLineTexureMode;
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

        
    }

   
    public void DefaultLineSetting(LineRenderer currentLine)
    {

        //apply default line setting
        currentLine.startWidth = defaultStartWidth;
        currentLine.endWidth = defaultEndWidth;
        currentLine.startColor = defaultStartColor;
        currentLine.endColor = defaultEndColor;
        currentLine.numCornerVertices = defaultCornerVertices;
        currentLine.numCapVertices = defaultEndCapVertices;
        currentLine.textureMode = defaultLineTexureMode;
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
}
