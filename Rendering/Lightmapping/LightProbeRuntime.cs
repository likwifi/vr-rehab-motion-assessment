using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightProbeRuntime : MonoBehaviour
{
    public Color m_Ambient;
    Light[] m_Lights;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;

        m_Lights = FindObjectsOfType<Light>();
        SphericalHarmonicsL2[] bakedProbes = LightmapSettings.lightProbes.bakedProbes;
        Vector3[] probePositions = LightmapSettings.lightProbes.positions;
        int probeCount = LightmapSettings.lightProbes.count;

        // Clear all probes
        for (int i = 0; i < probeCount; i++)
            bakedProbes[i].Clear();

        // Add ambient light to all probes
        for (int i = 0; i < probeCount; i++)
            bakedProbes[i].AddAmbientLight(m_Ambient);

        // Add directional and point lights' contribution to all probes
        foreach (Light l in m_Lights)
        {
            if (l.type == LightType.Directional)
            {
                for (int i = 0; i < probeCount; i++)
                    bakedProbes[i].AddDirectionalLight(-l.transform.forward, l.color, l.intensity);
            }
            else if (l.type == LightType.Point)
            {
                for (int i = 0; i < probeCount; i++)
                    SHAddPointLight(probePositions[i], l.transform.position, l.range, l.color, l.intensity, ref bakedProbes[i]);
            }
        }
    }
}
