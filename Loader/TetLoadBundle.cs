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

        var rootAssetPath = bundle.GetAllAssetNames()[0];
        var arObject = Instantiate(bundle.LoadAsset(rootAssetPath) as GameObject);
        bundle.Unload(false);
        /*var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Bundle");
        Instantiate(prefab);

        myLoadedAssetBundle.Unload(false);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
