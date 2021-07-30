using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    
    private Image UIimage;
    private RectTransform rectT;
    [SerializeField]
    private Material colorPickerMaterial;
    private static readonly int _AspectRatio = Shader.PropertyToID(nameof(_AspectRatio));

    //mouse position
    Vector2 mousePos;
    private void Awake()
    {
        UIimage = GetComponent<Image>();
        rectT = GetComponent<RectTransform>();
        UIimage.material = colorPickerMaterial;

     
    }

    private void Update()
    {
        
    }

    private bool IsOverColorPicker()
    {
        return false;
    }

    private Vector2 GetPickerPos(Vector2 position)
    {
        float start_x = rectT.position.x - (float)0.5 * rectT.rect.width;
        float start_y = rectT.position.y - (float)0.5 * rectT.rect.height;

        float x = (position.x - start_x) / (float)rectT.rect.width;  
        float y = (position.y - start_y) / (float)rectT.rect.height;
        Vector2 relPos = new Vector2(x, y);
        
        return relPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        mousePos = GetPickerPos(eventData.position);
        if ((mousePos.x > 0 && mousePos.x < 0.87) && (mousePos.y > 0 && mousePos.y < 1))
        {
            Shader.SetGlobalVector("_SVMousePos", mousePos);
          
        }else if((mousePos.x > 0.9 && mousePos.x < 1) && (mousePos.y > 0 && mousePos.y < 1))
        {
            Shader.SetGlobalVector("_HueMousePos", mousePos);
        }
    }



    public void OnDrag(PointerEventData eventData)
    {
        mousePos = GetPickerPos(eventData.position);
        if ((mousePos.x > 0 && mousePos.x < 0.85) && (mousePos.y > 0 && mousePos.y < 1))
        {
            Shader.SetGlobalVector("_SVMousePos", mousePos);

        }
        else if ((mousePos.x > 0.9 && mousePos.x < 1) && (mousePos.y > 0 && mousePos.y < 1))
        {
            Shader.SetGlobalVector("_HueMousePos", mousePos);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       //setcolor?
    }
}
