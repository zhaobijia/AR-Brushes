using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    
    private Image UIimage;

    [SerializeField]
    private Material colorPickerMaterial;
    private static readonly int _AspectRatio = Shader.PropertyToID(nameof(_AspectRatio));
    private void Awake()
    {
        UIimage = GetComponent<Image>();

        UIimage.material = colorPickerMaterial;

    }
   
 
}
