using UnityEngine;

public class Brush : MonoBehaviour
{
    public Material brushMaterial;

    BrushSetting brushSetting;

    private void Start()
    {
        brushSetting = FindObjectOfType<BrushSetting>();
        GetComponent<MeshRenderer>().material = brushMaterial;
    }

    public void SelectBrush()
    {
        brushSetting.lineMaterial = brushMaterial;
        brushSetting.ToggleBrushSet();
    }
}
