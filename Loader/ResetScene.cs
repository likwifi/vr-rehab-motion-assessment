using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // reloadScene();
    }

    public void reloadScene(string data)
    {
        Debug.Log("Destroyed Loaded Scene");
        // SPEECH
        // VoiceController.StopListening();
        StopAllCoroutines();
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Caching.ClearCache();
        // StartCoroutine(proceedReloadScene());
    }

    // private IEnumerator proceedReloadScene()
    // {
        // yield return new WaitForSeconds(7f);
        // GameObject loadedScene = GameObject.Find("LoadedScene");
        // if (loadedScene != null)
        // {
        //     Destroy(loadedScene);
        // }
        // StopAllCoroutines();
        // Debug.Log("Destroyed Loaded Scene");
        //
        // StopAllCoroutines();
        // var scene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(scene.name);
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
