using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTest : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Debug.Log("on draw gizmos?");
        Gizmos.color = Color.black;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
