using System.Collections;
using System.Collections.Generic;
using InteractiveModeScripts;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using utils;

public class WorkoutStarter : MonoBehaviour
{
    public API api;
    private Canvas _canvas;
    [CanBeNull] public GameObject poseEstimator;
    public static string userName;
    public static string gender;

    private CharacterControllerBase character;

    private void Awake()
    {
        // _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void Start()
    {
        Debug.Log("Loaded Scene");
        var loadedScene = new GameObject("LoadedScene");

        FlutterMessage.SendMessage("SCENE_INITIALIZED");

#if UNITY_EDITOR
        var nameUnityURL =
            "https://firebasestorage.googleapis.com/v0/b/optical-net-91011.appspot.com/o/bundles%2Fclasses%2Fhiit%2FJamie%2Fjamie-IOS?alt=media&token=add60b0b-cbca-4353-abd3-e9e5429f492b";
        api.GetBundleObject(nameUnityURL, OnContentLoaded, loadedScene.transform);
#endif
    }

    public void LoadJamieAssistant()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        FlutterMessage.SendMessage("Assistant_loaded");
    }

    public void ResetScene()
    {
        Debug.Log("Destroyed Loaded Scene");
        // SPEECH
        // VoiceController.StopListening();
        StopAllCoroutines();
        // var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
        Caching.ClearCache();
    }
}
