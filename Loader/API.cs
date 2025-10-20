using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using utils;

public class API : MonoBehaviour
{
 
    public void GetBundleObject(string assetName, UnityAction<GameObject,string> callback, Transform bundleParent) {
        StartCoroutine(GetDisplayBundleRoutine(assetName, callback, bundleParent));
    }
}
