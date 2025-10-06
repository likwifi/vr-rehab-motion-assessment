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
}
