using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLineRenderer : MonoBehaviour
{
    public float startRadius;
    public float endRadius;
    public Color startColor;
    public Color endColor;
    public Material material;
    public bool useWorldSpace;

    public int positionCount;

    public Cylinder cylinder;

    private void Awake()
    {
        cylinder = gameObject.AddComponent<Cylinder>();
        GetComponent<MeshRenderer>().materials[0] = material;
    }

    /// <summary>
    /// Set cylinder center positions
    /// </summary>
    /// <param name="index">cylinder center position index</param>
    /// <param name="pos">cylinder center position vector</param>
    public void SetPosition(int index, Vector3 pos)
    {

        cylinder.UpdateCylinderMesh(index, pos);
    }

    public void RenderPosition()
    {
        cylinder.Generate();
    }
}
