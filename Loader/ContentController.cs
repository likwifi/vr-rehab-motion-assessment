using UnityEngine;

public class ContentController : MonoBehaviour {

    public API api;

    public void LoadContent(string name) {
        DestroyAllChildren();
        name = "cybergirl-lesson-1";
        api.GetBundleObject(name, OnContentLoaded, transform);
    }
}
