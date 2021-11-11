using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    public int xSize, ySize, zSize;
    public int roundness;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        GenerateVertices();
        GenerateTriangles();
        
    }
    private void GenerateVertices()
    {
        int cornerVertices = 8;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = (
            (xSize - 1) * (ySize - 1) +
            (xSize - 1) * (zSize - 1) +
            (ySize - 1) * (zSize - 1)) * 2;
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        normals = new Vector3[vertices.Length];

        int v = 0;//vertex
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[v++] = new Vector3(x, y, 0);
            }

            for (int z = 1; z <= zSize; z++)
            {
                vertices[v++] = new Vector3(xSize, y, z);
            }

            for (int x = xSize-1; x >= 0; x--)
            {
                vertices[v++] = new Vector3(x, y, zSize);
            }
            for(int z = zSize-1; z > 0; z--)
            {
                vertices[v++] = new Vector3(0, y, z);
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
    }

    private void GenerateTriangles()
    {
        int triangle_number = (xSize * ySize + zSize * ySize)*2 * 2;
        int[] triangles = new int[triangle_number * 3];
        int ring = (xSize + zSize) * 2;

        int i = 0, y = 0, vx = 0;
        while (y < ySize)
        {
            int r = 0;
            while (r < ring - 1)
            {
                i = SetQuad(triangles, i, vx, vx + ring, vx + 1, vx + 1 + ring);
                vx++;
                r++;
            }
            //out of the loop, i stops at 
            i = SetQuad(triangles, i, vx, vx + ring, vx - ring + 1, vx + 1);
            vx++;
            y++;
        }
        mesh.triangles = triangles;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="triangles">Triangles integer array</param>
    /// <param name="i">triangle index</param>
    /// <param name="v00">vertex left bottom (0,0)</param>
    /// <param name="v01">vertex left top (0,1)</param>
    /// <param name="v10">vertex right bottom (1,0)</param>
    /// <param name="v11">vertex right top (1,1)</param>
    /// <returns></returns>
    private static int SetQuad(int[] triangles, int i, int v00, int v01, int v10, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i+3] = v01;
        triangles[i + 2] = triangles[i+5] =v10;
        triangles[i + 4] = v11;
        return i + 6; // return next starting position for 
    }
    
    private void OnDrawGizmos()
    {

        if(vertices == null)
        {
  
            return;
        }
        

        for(int i = 0; i<vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
    
}
