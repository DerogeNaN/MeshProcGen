using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] private List<MeshFilter> sourceMeshFilters;
    [SerializeField] private MeshFilter targetMeshFilter;
    [SerializeField] private MeshRenderer newMeshRenderer;
    [SerializeField] private ColourSettings oldPlanetColour;

    [ContextMenu(itemName: "Combine Meshes")]
    void CombineMeshes()
    {
        oldPlanetColour = GetComponentInParent<Planet>().colourSettings;
        
        CombineInstance[] combine = new CombineInstance[sourceMeshFilters.Count];

        for (int i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combine);

        foreach (MeshFilter meshFilter in sourceMeshFilters)
        {
            meshFilter.gameObject.SetActive(false);
        }

        

        targetMeshFilter.mesh = mesh;
        newMeshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        newMeshRenderer.sharedMaterial.color = oldPlanetColour.planetColour;
    }
}
