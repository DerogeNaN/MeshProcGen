using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseNoiseFilter : AbstractNoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings settings;

    public InverseNoiseFilter(NoiseSettings settings)
    {
        this.settings = settings;
    }

    public override float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.noiseLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.origin));
            v *= v;
            v *= weight;
            weight = v;

            noiseValue += ((v + 1) * 0.5f) * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}