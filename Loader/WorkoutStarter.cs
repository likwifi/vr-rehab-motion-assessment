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
        var nameUnityURL = ServiceConfig.EditorTestBundleUrl;
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

    public void LoadScene(string objectName)
    {
        // var m1 = new Message { id = 0, data = "Message received at unity side." };
        // Messages.Send(m1);

        // Message message = Messages.Receive(data);
        //
        // DestroyAllChildren();
        // var objectName = message.data;
        //
        GameObject loadedScene = new GameObject("LoadedScene");

        api.GetBundleObject(objectName, OnContentLoaded, loadedScene.transform);

        // message.data = "Loading Scene at object LoadedScene " + name;
        //
        // Messages.Send(message);
    }

    public void PlayPlaylist(string flutterMessage)
    {
        // var objectName = "rel1.mp3,rel2.mp3";
        // var type = "yoga";
        var m = JObject.Parse(flutterMessage);
        var objectName = (string)m.GetValue("music");
        var type = (string)m.GetValue("type");
        StartCoroutine(PlayPlaylist(objectName, type));
    }

    private IEnumerator PlayPlaylist(string objectName, string type)
    {
        string[] playlist = objectName.Split(",");
        foreach (var music in playlist)
        {
            yield return StartCoroutine(PlayMusic(music, type));
        }
    }

    private IEnumerator PlayMusic(string music, string type)
    {
        bool playbackFinished = false;
        var _cameraAudioSource = GetComponent<AudioSource>();

        // Start the coroutine to load and play the audio file
        StartCoroutine(api.LoadStreamingAudioFile(music, type, _cameraAudioSource, null,
            (obj) => { playbackFinished = true; }));

        // Wait until the audio file has finished playing
        yield return new WaitUntil(() => playbackFinished);
    }


    /*private IEnumerator PlayPlaylist(string objectName, string type)
    {
        var _cameraAudioSource = GetComponent<AudioSource>();
        string[] playlist = objectName.Split(",");
        var playbackFinished = true;
        foreach (var music in playlist)
        {
            yield return new WaitUntil(() => playbackFinished);
            playbackFinished = false;
            StartCoroutine(api.LoadStreamingAudioFile(music, type, _cameraAudioSource, null,
                (obj) => {
                playbackFinished = true; }));

            yield return new WaitForSeconds(5);
            StopAllCoroutines();
        }
    }*/

    public void setUserName(string userName_)
    {
        userName = userName_;
    }
    public void setGender(string gender_)
    {
        gender = gender_;
    }
    public void setLocaleCode(string localeCode_)
    {
        // localeCode = localeCode_;

        //
        // message.data = "Loading Scene at object LoadedScene " + name;
        //
        // Messages.Send(message);
    }


    void OnContentLoaded(GameObject content, string url)
    {
        /*else
        {
            character = GameObject.Find("Character").GetComponent<CharacterControllerBase>();
            character.StartResumeWorkout();
        }*/

        //do something cool here
        var message = "LOADED: " + content.name;

        Debug.Log("Unity: " + message);
        FlutterMessage.SendMessage(message);
        if (poseEstimator != null)
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            var poseEstimatorObj = Instantiate(poseEstimator, transform.position, transform.rotation);
            poseEstimatorObj.transform.parent = GameObject.Find("LoadedScene").transform;
            var character = GameObject.Find("Character").GetComponent<InteractiveCharacterController>();

            character.setWorkoutJson(ServiceConfig.EditorTestWorkoutJsonUrl);
#else
            StartCoroutine(WaitForWorkout());
#endif
        }
    }

    IEnumerator WaitForWorkout()
    {
        float startTime = Time.time;
        var character = GameObject.Find("Character").GetComponent<InteractiveCharacterController>();
        float maxWaitTime = 3.0f;
        while (!(character.WorkoutType.Equals("hiit") || character.WorkoutType.Equals("kids")) &&
               Time.time - startTime < maxWaitTime)
        {
            yield return null; // Wait for the next frame
        }

        if ((character.WorkoutType.Equals("hiit") || character.WorkoutType.Equals("kids")))
        {
            var poseEstimatorObj = Instantiate(poseEstimator, transform.position, transform.rotation);
            poseEstimatorObj.transform.parent = GameObject.Find("LoadedScene").transform;
        }
        else
        {
            Debug.Log("Timeout reached without matching the string value.");
        }
    }

    void DestroyAllChildren()
    {
        GameObject loadedScene = GameObject.Find("LoadedScene");
        if (loadedScene != null)
        {
            Destroy(loadedScene);
        }

        StopAllCoroutines();
        Debug.Log("Destroyed Loaded Scene");
    }

    // Update is called once per frame
    void Update()
    {
    }
}