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
        var ttsRequest = UnityWebRequest.PostWwwForm(ServiceConfig.SpeechServiceBaseUrl + "/speech/requestSpeech", "");
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
        
        
        var nameHash = Animator.StringToHash(speaker + text).ToString();
        var path = Path.Combine(Application.persistentDataPath, nameHash + ".mp3");
        File.WriteAllBytes(path, ttsRequest.downloadHandler.data);
        ttsRequest.Dispose();
        Debug.Log("Loaded");
        StartCoroutine(LoadAudioFile("file://" + path, (text) =>
        {
            Debug.Log("Playing");
            ToggleBgMusic(true);
            callbackOnPlay?.Invoke("finished");
        }, (text) =>
        {
            ToggleBgMusic(false);
            callbackOnFinishPlay?.Invoke("finished");
        }));
    }

    protected void ToggleBgMusic(bool lower)
    {
        if (_bgMusicAudioSource != null)
            _bgMusicAudioSource.volume = lower ? 0.2f : _bgMusicVolume;
    }


    protected void UpdateCounter()
    {
        // TODO interactive character controller case
        // timer.tim
        // if (progressBar == null)
        //     return;
        // countNumber += 1;
        // counter.text = countNumber.ToString();
        // progressBar.fillValue = (100 / totalCounts) * countNumber;
    }

    protected void StartCounter(float tickDuration, float totalDuration)
    {
        if (timer == null || totalDuration == 0)
            return;
        ResetCounter();
        timer.duration = totalDuration;
        timer.tickTime = timerType == "Countdown" ? 1 : tickDuration;
        timer.StartTimer();
    }

    protected void ResetCounter()
    {
        timer.StopAllCoroutines();
        timer.duration = 0;
        timer.tickTime = 0;
        timer.StopTimer();
    }

    // private bool isCoroutineExecuting;

    protected IEnumerator ExecuteAfterTime(float time, Action task)
    {
        // if (isCoroutineExecuting)
        // yield break;
        // isCoroutineExecuting = true;
        yield return new WaitForSeconds(time);
        task();
        // isCoroutineExecuting = false;
    }


    public virtual void StartResumeWorkout()
    {
        throw new NotImplementedException();
    }

    public static bool MovementsPaused { get; set; }

    protected virtual void Pause()
    {
        throw new NotImplementedException();
    }

    public IEnumerator speak(string text, Action<object> callbackOnPlay = null,
        Action<object> callbackOnFinishPlay = null, bool store = true)
    {
        yield return GetAudioClipService(text, callbackOnPlay, callbackOnFinishPlay, _speaker, _speakerStyle, store);
    }

    public void SetState(string state)
    {
        m_Animator.CrossFade(state, 0.5f);
    }

    public void setPoseEsimtator(PoseEstimation ps)
    {
        poseEstimation = ps;
    }

    public void BodyFullyVisible()
    {
        var userName = WorkoutStarter.userName;
        var message = string.Format(LocaleBundle.get()["can_see"], userName);

        StartCoroutine(speak(message, null, (object text) =>
        {
            FlutterMessage.SendMessage("hideSkipBtn", "ui");
            StartResumeWorkout();
        }));
        // instance.StartCoroutine(SpeechUtils.GetAudioClip(message,
        //     0f,
        //     _characterAudioSource, null, (object text) => { Resume(); }));
        // Destroy(_siriObj);
        // ToggleBgMusic(false);
        // VoiceController.StopListening();
    }

    public void BodyNotFullyVisible(Action continueWorkoutCallback)
    {
        var userName = WorkoutStarter.userName;
        var message = string.Format(LocaleBundle.get()["cant_see"], userName);
        Pause();
        StartCoroutine(speak(message, null, (object text) =>
        {
            FlutterMessage.SendMessage("showSkipBtn", "ui");
            // ListenForCallback(continueWorkoutCallback);
        }));
    }

    /*
    private void ListenForCallback(Action continueWorkoutCallback)
    {
        void Resume()
        {    StartCoroutine(speak(LocaleBundle.get()["resuming"], null, (object text) =>
            {
                StartResumeWorkout();
                continueWorkoutCallback();
            }));
            ToggleBgMusic(false);
            Destroy(_siriObj);
            VoiceController.StopListening();
        }
        void CallBack(string result)
        {
            var resultStr = (string)result;
            Debug.Log("Voice result is " + resultStr);
            var positiveWord = "continue";
            if (WorkoutStarter.localeCode == "zh")
            {
                positiveWord = "继续";
            }

            if (resultStr.IndexOf(positiveWord, StringComparison.OrdinalIgnoreCase) <= -1)
                return;
            Resume();
        }
        Debug.Log("Started Listening");
        var siri =  (GameObject) Resources.Load("SiriObj", typeof(GameObject));
        // render at place where no geometry exists.
        var loc = new Vector3(-1111.0270000007f, 1111.1170001f, -1111.17600012f);
        _siriObj = Instantiate(siri, loc, transform.rotation);
        ToggleBgMusic(true);
        VoiceController.StartListening(CallBack);
        // StartCoroutine( ExecuteAfterTime(5f, Resume));
       
#if UNITY_EDITOR
        // instance.StartCoroutine(SpeechUtils.GetAudioClip("Resuming workout.",
        //     0f,
        //     _characterAudioSource, null, (object text) =>
        //     {
        //         Resume();
        //         continueWorkoutCallback();
        //     }));
#endif
    }*/

    public void StartListening(Action<string> CallBack)
    {
        var siri = (GameObject)Resources.Load("SiriObj", typeof(GameObject));
        // render at place where no geometry exists.
        var loc = new Vector3(-1111.0270000007f, 1111.1170001f, -1111.17600012f);
        _siriObj = Instantiate(siri, loc, transform.rotation);
        var cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        cameraData.cameraStack.Add(GameObject.Find("Canvas Camera").GetComponent<Camera>());
        ToggleBgMusic(true);
// SPEECH
        // VoiceController.StartListening(CallBack);
    }

    public void StopListening()
    {
        Debug.Log("Stopped listening");
        ToggleBgMusic(false);
        Destroy(_siriObj);
        // SPEECH
        // VoiceController.StopListening();
    }
}