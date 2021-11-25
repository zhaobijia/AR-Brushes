using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cylinder : MonoBehaviour
{
    public List<Vector3> centerPos;
    public float radius = 0.1f;
    public int split_count = 10;
    public Color color;
    public Material material;
    public int positionCount;

    private int height = 0;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector2[] uvs;


    

    /// <summary>
    /// Update this Mesh for new add-in points 
    /// Generate new Mesh if no mesh yet
    /// </summary>
    /// <param name="newCenterIndex"></param>
    /// <param name="newCenterPos"></param>
    public void UpdateCylinderMesh(Vector3 newCenterPos)
    {

         centerPos.Add(newCenterPos);
         height = centerPos.Count;
         Generate();        
    }

    public void InitializeCylinder(Vector3 newCenterPos, float _radius, Color color, Material material)
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cylinder";
        
        //?
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = material;
        renderer.material.color = color;
        


        radius = _radius;

        centerPos = new List<Vector3>();
        centerPos.Add(newCenterPos);
        height = 1;

    }
    public void Generate()
    {
        GenerateVertices();
        GenerateTriangles();

    }

    private void GenerateVertices()
    {

        vertices = new Vector3[1 + (height - 2) * split_count + 1];
        normals = new Vector3[1 + (height - 2) * split_count + 1];
        uvs = new Vector2[vertices.Length];

        int v = 0, n =0;
        int u = 0;
        //starting point
        vertices[v++]= (centerPos[0]);
        normals[n++]=((centerPos[1] - centerPos[0]).normalized);
        uvs[u++] = Vector2.zero;
        //middle
        for (int h = 1; h < height - 1; h++)
        {
            for (int r = 0; r < split_count; r++)
            {
                //where this circle of vertices is set depends on the center,outer loop can be cut off when connecting to runtime drawing
                vertices[v++] = (CRCoordinateToXYZCoordinate(centerPos[h], centerPos[h] - centerPos[h - 1], radius, split_count, r));
                uvs[u++] = ConvertTextureToUVCoordinates(r, h, split_count, height);
                normals[n++] = ((vertices[v-1] - centerPos[h]).normalized);
                
            }
        }

        vertices[v] = (centerPos[height - 1]);
        normals[n] = (centerPos[height - 2] - centerPos[height - 1]);

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;

    }



    private void GenerateTriangles()
    {
        //+1 is starting and ending parts added together
        int triangle_number = split_count * 2 * (height+1);
        int[] triangles = new int[triangle_number * 3];

        if (height < 3)
        {

        }
        else
        {    
            //head:
            int hd = 0, tri = 0, h_v = 1;
            while (hd < split_count - 1)
            {
                tri = SetTriangle(triangles, tri, 0, h_v, h_v + 1);
                hd++;
                h_v++;
            }
            tri = SetTriangle(triangles, tri, 0, h_v, 1);
            
            //middle part
            
            int h = 0, v = 1;
            while (h < height - 3)
            {
                int q = 0;
                while (q < split_count - 1)
                {
                    tri = SetQuad(triangles, tri, v, v + split_count, v + 1, v + 1 + split_count);
                    q++;
                    v++;
                }
                //out of the loop, connect the last vertex of this level to the orgin (level below)'s starting vertex

                tri = SetQuad(triangles, tri, v, v + split_count, v - split_count + 1, v + 1);

                v++;
                h++;

            }

            
            int tl = 0, t_v = 0;
            while (tl < split_count - 1)
            {
                tri = SetTriangle(triangles, tri, v + t_v, v + split_count, v + 1 + t_v);
                tl++;
                t_v++;
            }
            tri = SetTriangle(triangles, tri, v + t_v, v + split_count, v);
            
        }

        mesh.triangles = triangles;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"> center of circle</param>
    /// <param name="dir">direction to where the circle faces</param>
    /// <param name="radius">radius of circle</param>
    /// <param name="count">count of sector for the circle</param>
    /// <param name="i">index of sector</param>
    /// <returns></returns>
    private Vector3 CRCoordinateToXYZCoordinate(Vector3 center, Vector3 dir, float radius, int count, int i)
    {
        Vector3 v00_dir;
        //check if dir is close to vector3.down:
        if (Vector3.Dot(dir.normalized, Vector3.down) > 0.9f)
        {
            Quaternion qua = Quaternion.FromToRotation(Vector3.down, dir);

            v00_dir = qua * Vector3.left;
        }
        else
        {

            Quaternion qua = Quaternion.FromToRotation(Vector3.up, dir);

            v00_dir = qua * Vector3.right;
        }

        float angle = -i * (360 / count);
        
        Vector3 i_dir = Quaternion.AngleAxis(angle, dir) * v00_dir;

        Vector3 pos = center + i_dir * radius;
        return pos;
    }


    private Vector2 ConvertTextureToUVCoordinates(int x, int y, int width, int height)
    {
        Vector2 result;

        float x_ = x * (1 /(float)width);
        float y_ = y * (1 / (float)height);
        result = new Vector2(x_, y_);
        return result;
    }

    /// <summary>
    /// Draw Quad base on 4 vertices
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
        triangles[i + 1] = triangles[i + 3] = v01;
        triangles[i + 2] = triangles[i + 5] = v10;
        triangles[i + 4] = v11;
        return i + 6; // return next starting position for 
    }

    private static int SetTriangle(int[] triangles, int i, int v00, int v01, int v10)
    {
        triangles[i] = v00;
        triangles[i + 1] = v01;
        triangles[i + 2] = v10;
        return i + 3;
    }



    private void OnDrawGizmos()
    {

        if (vertices == null)
        {
            return;
        }


        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.01f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]*0.1f);
        }
    }
}
