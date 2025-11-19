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
        loader.LoadScene("https://firebasestorage.googleapis.com/v0/b/optical-net-91011.appspot.com/o/classes%2Fcybergirl_lesson_1-IOS?alt=media&token=c62116ce-56c8-4cae-b841-cb03d1944949");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
