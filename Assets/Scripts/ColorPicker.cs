using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    
    private Image UIimage;
    private RectTransform rectT;

    [SerializeField]
    private Material colorPickerMaterial;


    public Color pickedColor;
    private float hue;
    private float saturation;
    private float value;

    //mouse position, pass this to shader
    Vector2 mousePos;
    private void Awake()
    {
        UIimage = GetComponent<Image>();
        rectT = GetComponent<RectTransform>();
        UIimage.material = colorPickerMaterial;

     
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
        SetMousePosToShader(mousePos);
    }



    public void OnDrag(PointerEventData eventData)
    {
        mousePos = GetPickerPos(eventData.position);
        SetMousePosToShader(mousePos);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    private void SetMousePosToShader(Vector2 mousePos)
    {
        if ((mousePos.x > 0 && mousePos.x < 0.85) && (mousePos.y > 0 && mousePos.y < 1))
        {
            //on sv block/area
            Shader.SetGlobalVector("_SVMousePos", mousePos);
            saturation = mousePos.x/0.85f;
            value = mousePos.y;
            pickedColor = GetColorFromMousePos();
        }
        else if ((mousePos.x > 0.9 && mousePos.x < 1) && (mousePos.y > 0 && mousePos.y < 1))
        {
            //on hue area
            Shader.SetGlobalVector("_HueMousePos", mousePos);
            hue = mousePos.y;
            pickedColor = GetColorFromMousePos();
        }
    }


    private Color GetColorFromMousePos()
    {
        return Color.HSVToRGB(hue, saturation, value);
    }

}
