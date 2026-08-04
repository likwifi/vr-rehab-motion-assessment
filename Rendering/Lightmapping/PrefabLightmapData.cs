using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class PrefabLightmapData : MonoBehaviour
{
    [Tooltip("Reassigns shaders when applying the baked lightmaps. Might conflict with some shaders like transparent HDRP.")]
    public bool releaseShaders = true;
    
    [System.Serializable]
    struct RendererInfo
    {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;
    }
    [System.Serializable]
    struct LightInfo
    {
        public Light light;
        public int lightmapBaketype;
        public int mixedLightingMode;
    }

    [SerializeField]
    Material skybox;
    
    [SerializeField]
    RendererInfo[] m_RendererInfo;
    [SerializeField]
    Texture2D[] m_Lightmaps;
    [SerializeField]
    Texture2D[] m_LightmapsDir;
    [SerializeField]
    Texture2D[] m_ShadowMasks;
    [SerializeField]
    LightInfo[] m_LightInfo;


    void Awake()
    {
      
        // RenderSettings.ambientLight = Color.blue;
        // RenderSettings.ambientMode = AmbientMode.Custom;
        
        // RenderSettings.ambientLight = Color.yellow;
        // RenderSettings.ambientSkyColor = Color.yellow;
        // RenderSettings.ambientGroundColor = Color.yellow;
        // RenderSettings.ambientEquatorColor = Color.yellow;

        // RenderSettings.ambientEquatorColor
        // RenderSettings.ambientGroundColor
        // DynamicGI.UpdateEnvironment();

        Init();
    }

    private void Start()
    {
        if (skybox != null)
        {
            RenderSettings.skybox = skybox;
            DynamicGI.UpdateEnvironment();
        }
    }

    void Init()
    {
        if (m_RendererInfo == null || m_RendererInfo.Length == 0)
            return;

        var lightmaps = LightmapSettings.lightmaps;
        int[] offsetsindexes = new int[m_Lightmaps.Length];
        int counttotal = lightmaps.Length;
        List<LightmapData> combinedLightmaps = new List<LightmapData>();

        for (int i = 0; i < m_Lightmaps.Length; i++)
        {
            bool exists = false;
            for (int j = 0; j < lightmaps.Length; j++)
            {

                if (m_Lightmaps[i] == lightmaps[j].lightmapColor)
                {
                    exists = true;
                    offsetsindexes[i] = j;

                }

            }
            if (!exists)
            {
                offsetsindexes[i] = counttotal;
                var newlightmapdata = new LightmapData
                {
                    lightmapColor = m_Lightmaps[i],
                    lightmapDir = m_LightmapsDir.Length == m_Lightmaps.Length ? m_LightmapsDir[i] : default(Texture2D),
                    shadowMask = m_ShadowMasks.Length == m_Lightmaps.Length  ? m_ShadowMasks[i] : default(Texture2D),
                };

                combinedLightmaps.Add(newlightmapdata);

                counttotal += 1;


            }

        }

        var combinedLightmaps2 = new LightmapData[counttotal];

        lightmaps.CopyTo(combinedLightmaps2, 0);
        combinedLightmaps.ToArray().CopyTo(combinedLightmaps2, lightmaps.Length);

        bool directional=true;

        foreach(Texture2D t in m_LightmapsDir)
        {
            if (t == null)
            {
                directional = false;
                break;
            }
        }
    }
}
