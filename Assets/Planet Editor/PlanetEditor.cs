using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;

    private void OnEnable()
    {
        planet = (Planet)target;
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool dropDown, ref Editor curEditor)
    {
        if(settings != null)
        {
            dropDown = EditorGUILayout.InspectorTitlebar(dropDown, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (dropDown)
                {
                    CreateCachedEditor(settings, null, ref curEditor);
                    curEditor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate!"))
        {
            planet.GeneratePlanet();
        }

        if (GUILayout.Button("WeldEdges!"))
        {
            planet.CombineMeshes();
        }
            
        DrawSettingsEditor(planet.colourSettings, planet.OnColourUpdate, ref planet.colourSettingsDropDown, ref colourEditor);
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeUpdate, ref planet.shapeSettingsDropDown, ref shapeEditor);
    }
}