using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractNoiseFilter 
{
    public abstract float Evaluate(Vector3 point);
}

public static class NoiseFactory
{
    public static AbstractNoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings)
    {
        switch (noiseSettings.noiseType)
        {
            case NoiseType.Simple: 
                return new SimpleNoiseFilter(noiseSettings);
            case NoiseType.Inverse: 
                return new InverseNoiseFilter(noiseSettings);
            default: return null;
        }
    }
}