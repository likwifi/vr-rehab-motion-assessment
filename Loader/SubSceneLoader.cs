using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubSceneLoader : MonoBehaviour
{
    //public API api;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LoadScene()
    {

        SceneManager.UnloadSceneAsync("Loader");
        Scene nextScene = SceneManager.GetSceneByName("Loader");
            if (nextScene.isLoaded) // this will never become true
            {
                Scene activeScene = SceneManager.GetActiveScene();
                //SceneManager.UnloadScene(activeScene.buildIndex);
                SceneManager.SetActiveScene(nextScene);
                //nextSceneName = null;
            }
        
    }

    public void LoadScene(string subScenePath)
    {
        //SceneManager.LoadScene("Loader");
        //FlutterUnityPlugin.Message message = FlutterUnityPlugin.Messages.Receive(data);
        //string subScenePath = message.data;
        //GameObject loadedScene = new GameObject("Loaded");
        //GameObject Controller1 = GameObject.Find("Controller1");
        //ScriptB controller = (ScriptB)go.GetComponent(typeof(ScriptB));
        //controller.load
        //api.GetBundleObject(subScenePath, OnContentLoaded, loadedScene.transform);
    }

    void OnContentLoaded(GameObject content)
    {
        //do something cool here
        Debug.Log("Loaded: " + content.name);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
