using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

public static class SpeechUtils
{
    public static IEnumerator GetAudioClip(string text, float waitTime,
        AudioSource characterAudioSource,
        [CanBeNull] Action<object> callbackOnPlay,
        [CanBeNull] Action<object> callbackOnFinishPlay)
    {
        var url = "http://185.127.66.13:8889/synth-audio?text=" + UnityWebRequest.EscapeURL(text);
        using var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
    }
}
