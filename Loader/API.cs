using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using utils;

public class API : MonoBehaviour
{
 
    public void GetBundleObject(string assetName, UnityAction<GameObject,string> callback, Transform bundleParent) {
        StartCoroutine(GetDisplayBundleRoutine(assetName, callback, bundleParent));
    }
    IEnumerator GetDisplayBundleRoutine(string assetName, UnityAction<GameObject,string> callback, Transform bundleParent) {
        var bundleURL = assetName;
        Debug.Log("Requesting bundle at " + bundleURL);


        //request asset bundle
        var www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        var operation = www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log("Network error");
        } else {
            while (!operation.isDone)
            {
                var downloadDataProgress = www.downloadProgress * 100;
                Debug.Log(downloadDataProgress.ToString());
                FlutterMessage.SendMessage(downloadDataProgress.ToString(), "loadProgress");
                yield return null;
            }
            
            var bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle != null) {
                var rootAssetPath = bundle.GetAllAssetNames()[0];
                var arObject = Instantiate(bundle.LoadAsset(rootAssetPath) as GameObject,bundleParent);
                bundle.Unload(false);
                callback(arObject, bundleURL);
            } else {
                Debug.Log("Not a valid asset bundle");
            }
            www.Dispose();
        }
    }


    public IEnumerator LoadStreamingAudioFile(string fileName, string type, AudioSource audioSource,
        Action<object> callbackOnPlay, Action<object> callbackOnFinish)
    {
        using var www = UnityWebRequestMultimedia.GetAudioClip(ServiceConfig.SpeechServiceBaseUrl + "/speech/requestMusicByType?fileName="+fileName+"&type="+type, AudioType.MPEG);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = myClip;
            audioSource.Play();
            Debug.Log("Audio is playing.");
            callbackOnPlay?.Invoke("success");
            var length = myClip.length;
            yield return new WaitForSeconds(length/myClip.channels);
            callbackOnFinish?.Invoke("success");
        }
    }

}