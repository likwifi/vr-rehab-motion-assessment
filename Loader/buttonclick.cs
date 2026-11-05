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

        string bundleURL = ServiceConfig.SalsaLessonBundleUrl;
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


    public void LoadScene1(string name)
    {
        //data = "{\"name\": \"aaa\"}";
        //FlutterUnityPlugin.Message message = FlutterUnityPlugin.Messages.Receive(data);

        DestroyAllChildren();
        //name = message.data;

        
        GameObject loadedScene = new GameObject("LoadedScene");
        api.GetBundleObject(name, Loaded, loadedScene.transform);

        //message.data = "Loading Scene at object LoadedScene " + name;

        //FlutterUnityPlugin.Messages.Send(message);
    }


    public void CreateScene() {
        //Debug.Log("Clicked CREATE");
        //go1 = new GameObject("LoadedScene");
        string bundleURL = ServiceConfig.ForestLessonBundleUrl;
        //api.GetBundleObject(bundleURL, Loaded, go1.transform);
        LoadScene1(bundleURL);
    }

    private void Loaded(GameObject arg0, string url)
    {
        throw new NotImplementedException();
    }

}
