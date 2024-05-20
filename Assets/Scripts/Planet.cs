using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)] 
    public int resolution = 10;

    [SerializeField , HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private void OnValidate()
    {
        Initialize();   // need to make sure that meshes aren't initialized when they already exist. See comments below.
        GenerateMesh();
    }

    void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0) // only need to create mesh filters if array doesn't exist or is empty.
        {
            meshFilters = new MeshFilter[6];    
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null) // only initialize a new mesh object when there isn't already one.
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}