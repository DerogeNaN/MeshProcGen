using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TerrainFace
{
    Mesh mesh;
    public MeshWelder meshWelder;
    int meshResolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    ShapeGenerator shapeGenerator;

    Vector3[] edgeVerts;

    public TerrainFace(Mesh mesh, int meshResolution, Vector3 localUp, ShapeGenerator shapeGenerator)
    {
        this.mesh = mesh;
        this.meshResolution = meshResolution;
        this.localUp = localUp;
        this.shapeGenerator = shapeGenerator;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);

        edgeVerts = new Vector3[(meshResolution - 1) * 4];
    }

    public void ConstructMesh()
    {
        Vector3[] verticies = new Vector3[meshResolution * meshResolution];
        int[] triangles = new int[(meshResolution - 1) * (meshResolution - 1) * 6];
        int triIndex = 0;

        int i = 0;
        for (int y = 0; y < meshResolution; y++)
        {
            for (int x = 0; x <meshResolution; x++)
            {
                Vector2 progressPercent = new Vector2(x, y) / (meshResolution - 1);
                Vector3 pointOnUnitCube = localUp + (progressPercent.x - 0.5f) * 2 * axisA + (progressPercent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                verticies[i] = shapeGenerator.CalcPointOnPlanet(pointOnUnitSphere);

                if (x == 0 || y == 0 || x == meshResolution - 1 || y == meshResolution - 1)
                {
                    CreateSphereAtPoint(verticies[i]);
                    CreateEdgeArray(verticies[i]);
                }

                if (x != meshResolution - 1 && y != meshResolution -1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + meshResolution + 1;
                    triangles[triIndex + 2] = i + meshResolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + meshResolution + 1;
                    triIndex += 6;
                }
                i++;
            }
        }

        PopulateMeshWelder();

        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void CreateSphereAtPoint(Vector3 point)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = point;
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) / meshResolution;
    }

    void CreateEdgeArray(Vector3 vertToAdd)
    {
        for (int i = 0; i < edgeVerts.Length; i++)
        {
            if (edgeVerts[i] == Vector3.zero)
            {
                edgeVerts[i] = vertToAdd;
                break;
            }
        }
    }
    void PopulateMeshWelder()
    {
        for (int i = 0; i < meshWelder.edgeVertArrays.Length; i++) 
        {
            if (meshWelder.edgeVertArrays[i] == null)
            {
                meshWelder.edgeVertArrays[i] = edgeVerts;
                break;
            }
        }
    }
}
