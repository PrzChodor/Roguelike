using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Vision : MonoBehaviour
{
    public int rays = 360;
    public MeshFilter meshFilter;
    public LayerMask obstacleMask;

    public float edgeDistThreshold;
    public int edgeDetectionIterations;

    private Vector2 dir;
    private Mesh mesh;

    private void Start()
    {
        dir = Vector2.up;
        mesh = new Mesh();
        mesh.name = "Vision Mesh";
        meshFilter.mesh = mesh;
    }

    void Update()
    {
        var points = new List<Vector3>();
        var oldRay = new RaycastHit2D();
        for (int i = 0; i < rays; i++)
        {
            var ray = Physics2D.Raycast(transform.position, dir, 100, obstacleMask);

            if(i > 0 && Mathf.Abs(Vector2.Distance(ray.point,oldRay.point)) > edgeDistThreshold)
            {
                var angle = 360f / ((edgeDetectionIterations + 1) * rays);
                var tempDir = dir.Rotate(-(edgeDetectionIterations + 1) * angle);

                for (int j = 0; j < edgeDetectionIterations; j++)
                {
                    tempDir = tempDir.Rotate(angle);
                    points.Add(Physics2D.Raycast(transform.position, tempDir, 100, obstacleMask).point);
                }
            }

            points.Add(ray.point);
            oldRay = ray;
            dir = dir.Rotate(360f / rays);
        }

        int vertexCount = points.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 1) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(points[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            else
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = 1;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

}
