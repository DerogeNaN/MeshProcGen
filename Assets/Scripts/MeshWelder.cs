using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshWelder
{
    public Vector3[][] edgeVertArrays = new Vector3[6][];

    //welding edge verts logic was going to go here. Decided to utilize Unity's Mesh.CombineMeshes 01/06/24.

    public bool CheckMatch(Vector3 vertA, Vector3 vertB)
    {
        if (vertA == vertB) return true;
        else return false;
    }
}
