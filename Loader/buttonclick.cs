using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class buttonclick : MonoBehaviour
{
    public API api;
    [FormerlySerializedAs("start")] public WorkoutStarter workoutStarter;
    GameObject go1;
    public void DestroyScene()
    {
        //Debug.Log("Clicked DESTROY");
        //GameObject controller = GameObject.Find("LoadedScene");
        //if (controller != null) {
        //    GameObject.Destroy(controller);
        //}

        string bundleURL = "https://firebasestorage.googleapis.com/v0/b/optical-net-91011.appspot.com/o/classes%2Fsalsa_lesson_1%2Fcybergirl_lesson_1-IOS?alt=media&token=44b40d50-9ccd-4346-95ce-83b102f27a19";
        //api.GetBundleObject(bundleURL, Loaded, go1.transform);
        LoadScene1(bundleURL);

    }

    void DestroyAllChildren()
    {
        GameObject loadedScene = GameObject.Find("LoadedScene");
        if (loadedScene != null)
        {
            GameObject.Destroy(loadedScene);
        }
        Debug.Log("Destroyed Loaded Scene");
    }
}
