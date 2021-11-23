using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLineRenderer : MonoBehaviour
{
    [HideInInspector] public float radius;
    [HideInInspector] public Color color = Color.white;
    [HideInInspector] public Material material;
    [HideInInspector] public int positionCount;

    public Cylinder cylinder;

    private void Awake()
    {
        cylinder = gameObject.AddComponent<Cylinder>();
        GetComponent<MeshRenderer>().materials[0] = material;
    }

    public void InitializeRenderer(Vector3 pos)
    {
        cylinder.InitializeCylinder(pos, radius, color, material);
    }

    /// <summary>
    /// Set cylinder center positions
    /// </summary>
    /// <param name="index">cylinder center position index</param>
    /// <param name="pos">cylinder center position vector</param>
    public void SetPosition(Vector3 pos)
    {

        cylinder.UpdateCylinderMesh(pos);
    }

    public void RenderPosition()
    {
        cylinder.Generate();
    }
}
