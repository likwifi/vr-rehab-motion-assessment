using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public SubSceneLoader loader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene() {
        loader.LoadScene(ServiceConfig.DemoLessonBundleUrl);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
