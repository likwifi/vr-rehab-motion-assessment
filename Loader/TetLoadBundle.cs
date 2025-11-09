using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TetLoadBundle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "arthur_baked-IOS"));
        if (bundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
    }
}
