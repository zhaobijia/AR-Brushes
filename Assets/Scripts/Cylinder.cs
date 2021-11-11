using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cylinder : MonoBehaviour
{
    public Vector3 center;
    public int radius;
    public int split_count;
    public int height;
    public int edge;

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
        mesh.name = "Procedural Cylinder";

        GenerateVertices();
        GenerateTriangles();

    }

    private void GenerateVertices() 
    {
        //+2 are starting and ending points
        vertices = new Vector3[split_count * (height + 1) + 2];
        normals = new Vector3[split_count * (height + 1) + 2];
        int v = 0,n = 0;
        //starting point
        vertices[v++] = center - Vector3.up*edge;
        normals[n++] = Vector3.down;

        for (int h = 0; h <= height; h++)
        {
            for(int r = 0; r <split_count; r++)
            {
                //where this circle of vertices is set depends on the center,outer loop can be cut off when connecting to runtime drawing
                vertices[v++] = CRCoordinateToXYZCoordinate(center + Vector3.up*h, Vector3.up, radius, split_count, r);
                
                normals[n++] = (vertices[v - 1] - (center + Vector3.up * h)).normalized;
            }
        }

        vertices[v++] = center + (Vector3.up * height) + Vector3.up * edge;
        normals[n++] = Vector3.up;
        
        mesh.vertices = vertices;
        mesh.normals = normals;
    }


    private void GenerateTriangles() 
    {
        //+1 is starting and ending parts added together
        int triangle_number = split_count * 2 * (height+1);
        int[] triangles = new int[triangle_number*3 ];


        //head:
        int hd = 0, tri = 0, h_v = 1;
        while(hd < split_count-1)
        {
            tri = SetTriangle(triangles, tri, 0, h_v, h_v + 1);
            hd++;
            h_v++;
        }
        tri = SetTriangle(triangles, tri, 0, h_v, 1);

        int h = 0,  v=1;
        while (h < height)
        {
            int q = 0;
            while(q < split_count-1)
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
        while(tl < split_count-1)
        {
            tri = SetTriangle(triangles, tri, v + t_v, v+split_count, v + 1 + t_v);
            tl++;
            t_v++;
        }
        tri = SetTriangle(triangles, tri, v + t_v, v + split_count, v);
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
    private Vector3 CRCoordinateToXYZCoordinate(Vector3 center, Vector3 dir, int radius, int count, int i) {

        Quaternion qua = Quaternion.FromToRotation(Vector3.up, dir);
        Vector3 v00_dir = qua * Vector3.right; //Direction from center to vertex, for angle at 0;
        float angle = -i*(360 / count);
        Vector3 i_dir = Quaternion.AngleAxis(angle, dir)*v00_dir;

        Vector3 pos = center + i_dir *radius;
        return pos;
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
            Gizmos.DrawSphere(vertices[i], 0.1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
}
