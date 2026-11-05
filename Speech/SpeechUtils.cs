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
        var url = ServiceConfig.SynthAudioUrl + "?text=" + UnityWebRequest.EscapeURL(text);
        using var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var myClip = DownloadHandlerAudioClip.GetContent(www);
            characterAudioSource.clip = myClip;
            characterAudioSource.Play();
            Debug.Log("Audio is playing.");
            callbackOnPlay?.Invoke(myClip);
        }

        www.Dispose();
        yield return new WaitWhile(() => characterAudioSource.isPlaying);
        callbackOnFinishPlay?.Invoke("finished");
        yield return new WaitForSeconds(waitTime);
    }
}