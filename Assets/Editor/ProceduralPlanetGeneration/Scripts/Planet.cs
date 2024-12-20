using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public bool autoUpdate = true;

    [Range(2, 255)] 
    public int resolution = 10;

    [SerializeField , HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    ShapeGenerator shapeGenerator;
    //public MeshWelder welder;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector] public bool shapeSettingsDropDown;
    [HideInInspector] public bool colourSettingsDropDown;

    private void BuildMesh()
    {
        GeneratePlanet();   //need to make sure that meshes aren't initialized when they already exist. See comments below.
    }

#if UNITY_EDITOR            //this removes a silly unity warning.
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += BuildMesh;
    }
#endif

    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

        //welder = new MeshWelder(resolution);

        if (meshFilters == null || meshFilters.Length == 0) // only need to create mesh filters if array doesn't exist or is empty (no double ups).
        {
            meshFilters = new MeshFilter[6];    
        }

        terrainFaces = new TerrainFace[6];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null) // only initialize a new mesh object when there isn't already one (no double ups).
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.transform.localPosition = Vector3.zero;
                meshObj.transform.localRotation = Quaternion.identity;
                meshObj.transform.localScale = Vector3.one;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i], shapeGenerator);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            //PopulateMeshWelder();
            face.ConstructMesh();
        }
    }

    void GenerateColours()
    {
        foreach (MeshFilter meshfilter in meshFilters)
        {
            meshfilter.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
    }

    //public void RecalculateEdgeNormals()
    //{
    //    List<int> matchedIndices = new List<int>();
    //
    //    for (int i = 0; i < welder.edgeVertArray.Length; i++)
    //    {
    //        for (int j = 0; j < welder.edgeVertArray.Length; j++)
    //        {
    //            if (welder.CheckMatch(welder.edgeVertArray[j], welder.edgeVertArray[i]))
    //            {
    //                matchedIndices.Add(j);
    //            }
    //            else continue;
    //        }
    //    }
    //}

    //void PopulateMeshWelder(TerrainFace curFace)
    //{
    //    if (shapeGenerator.shapeSettings.populateMeshWelderArray)
    //    {
    //        for (int i = 0; i < welder.edgeVertArray.Length; i++)
    //        {
    //            for (int j = 0; j < curFace.edgeVerts.Length; j++)
    //            {
    //                if (welder.edgeVertArray[i] == Vector3.zero)
    //                {
    //                    welder.edgeVertArray[i] = curFace.edgeVerts[j];
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < welder.edgeVertArray.Length; i++)
    //        {
    //            for (int j = 0; j < curFace.edgeVerts.Length; j++)
    //            {
    //                if (welder.edgeVertArray[i] != Vector3.zero)
    //                {
    //                    welder.edgeVertArray[i] = curFace.edgeVerts[j];
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //}
}