using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeralPhluid : MonoBehaviour
{
    [SerializeField] Planet planet;
    [SerializeField] Vector3 velocity;

    void Update()
    {
        planet.shapeSettings.noiseFilters[0].noiseSettings.origin += velocity * Time.deltaTime;
        planet.GeneratePlanet();
    }
}
