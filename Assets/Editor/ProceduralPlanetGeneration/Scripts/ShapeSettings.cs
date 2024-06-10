using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    //public bool createEdgeArray;
    //public bool populateMeshWelderArray;

    public float planetRadius = 1.0f;
    public NoiseLayer[] noiseFilters;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool firstLayerShouldMask;
        public NoiseSettings noiseSettings;
    }
}