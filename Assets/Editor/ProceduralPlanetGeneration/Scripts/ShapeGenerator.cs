using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    public ShapeSettings shapeSettings;
    AbstractNoiseFilter[] noiseFilters;
    public ShapeGenerator(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
        noiseFilters = new AbstractNoiseFilter[shapeSettings.noiseFilters.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFactory.CreateNoiseFilter(shapeSettings.noiseFilters[i].noiseSettings);
        }
    }

    public Vector3 CalcPointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float elevation = 0;
        float firstLayerValue = 0;

        if (noiseFilters.Length > 0) 
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (shapeSettings.noiseFilters[0].enabled) elevation = firstLayerValue;
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            float mask = (shapeSettings.noiseFilters[i].firstLayerShouldMask) ? firstLayerValue : 1;
            if (shapeSettings.noiseFilters[i].enabled) elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
        }
        return pointOnUnitSphere * shapeSettings.planetRadius * (1 + elevation);
    }
}