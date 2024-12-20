using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public NoiseType noiseType;
    
    public float strength = 1;
    public float roughness = 1;
    public float baseRoughness = 1;
    public float persistence = 0.5f;
    public float minValue;

    [Range(1, 8)] public int noiseLayers = 1;

    public Vector3 origin;
}

public enum NoiseType
{
    Simple,
    Inverse
}