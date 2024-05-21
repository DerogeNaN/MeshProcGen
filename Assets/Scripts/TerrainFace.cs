using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;
    int meshResolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    ShapeGenerator shapeGenerator;

    public TerrainFace(Mesh mesh, int meshResolution, Vector3 localUp, ShapeGenerator shapeGenerator)
    {
        this.mesh = mesh;
        this.meshResolution = meshResolution;
        this.localUp = localUp;
        this.shapeGenerator = shapeGenerator;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
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
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
