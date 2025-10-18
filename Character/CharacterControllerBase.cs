using System;
using System.Collections;
using System.IO;
using System.Text;
using InteractiveModeScripts.obj;
using JetBrains.Annotations;
using Mediapipe.Unity.Tutorial;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using utils;

public class CharacterControllerBase : MonoBehaviour
{
    protected AudioSource characterAudioSource;
    private AudioSource _bgMusicAudioSource;
    private float _bgMusicVolume;
    protected GameObject mainCamera;

    protected Animator m_Animator;
    private CircularTimer timer;
    protected int moveIndex = 0;
    protected MovesList<InteractionObject> moves;
    private GameObject _siriObj;
    protected string timerType = "";

    private int _countNumber = 0;
    protected int totalCounts = 0;

    public GameObject ProgressBarGameObject1;
    public string currentAnimatorState;
    public InteractionObject currentInteraction;
    public PoseEstimation poseEstimation;

    [CanBeNull] public string _speaker;
    [CanBeNull] public string _speakerStyle;
    [CanBeNull] public string localeCode;

    public static bool isInitialized = false;
    // public GameObject poseEstimator;

    private void Awake()
    {
        characterAudioSource = GetComponent<AudioSource>();
        var controllerGameObj = GameObject.Find("Controller");
        if (controllerGameObj != null)
        {
            _bgMusicAudioSource = controllerGameObj.GetComponent<AudioSource>();
            if (_bgMusicAudioSource != null)
            {
                _bgMusicVolume = _bgMusicAudioSource.volume;
            }
        }

        m_Animator = GetComponent<Animator>();
        if (ProgressBarGameObject1 != null)
        {
            timer = ProgressBarGameObject1.GetComponent<CircularTimer>();
            ProgressBarGameObject1.SetActive(false);
        }
    }

    public Animator getAnimator()
    {
        return m_Animator;
    }

    protected virtual IEnumerator WorkoutRunner(InteractionObject interaction)
    {
        throw new NotImplementedException();
    }

    private IEnumerator LoadAudioFile(string path, Action<object> callbackOnPlay, Action<object> callbackOnFinish)
    {
        using var www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        var result = www.SendWebRequest();

        yield return new WaitUntil(() => result.isDone);
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
            // return null;
        }
        else
        {
            var myClip = DownloadHandlerAudioClip.GetContent(www);

            characterAudioSource.clip = myClip;
            characterAudioSource.Play();
            Debug.Log("Audio is playing.");
            callbackOnPlay?.Invoke("success");
            var length = myClip.length;
            yield return new WaitForSeconds(length);
            callbackOnFinish?.Invoke("success");
        }
    }

    protected IEnumerator GetAudioClipService(string text, Action<object> callbackOnPlay,
        Action<object> callbackOnFinishPlay, string speaker = "Jenny", string style = "", bool store=true)
    {
        if (text.Equals(""))
        {
            callbackOnFinishPlay?.Invoke("finished");
            yield break;
        }

        // var localeCode = localeCode;
        // localeCode = localeCode == "zh" ? "yue-CN" : "";
        var body = "{\"text\": \"" + text + "\",\"speaker\": \"" + speaker + "\"" +
                   ",\"style\": \"" + style + "\", \"locale\": \"" + localeCode + "\",\"store\":\""+store+"\"}";
        
        Debug.Log(text);
        // https://jamies.fit/api/requestSpeech
        // http://localhost:3000/requestSpeech
        var ttsRequest = UnityWebRequest.PostWwwForm("https://api2-wpjhixwfcq-uc.a.run.app/speech/requestSpeech", "");
        ttsRequest.SetRequestHeader("Content-Type", "application/json");
        ttsRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(@body));
        yield return ttsRequest.SendWebRequest();
        if (ttsRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Request Error: {ttsRequest.error}\nResponse: {ttsRequest.downloadHandler.text}");
           yield return null;
        }
        else
        {
            Debug.Log($"Request Successful. Response: {ttsRequest.downloadHandler.text}");
        }
    }
}
