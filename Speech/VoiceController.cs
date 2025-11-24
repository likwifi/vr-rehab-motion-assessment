// using System;
// using FlutterUnityIntegration;
// using TextSpeech;
// using TMPro;
// using UnityEngine;
// using utils;
//
// public class VoiceController : MonoBehaviour
// {
//     private static Action<string> finalCallback;
//     
//     private static Action<string> partialCallback;
//     private const string LANG_CODE = "en-US";
//     private const string LANG_CODE_ZH = "zh-CN";
//
//     [SerializeField] public TextMeshProUGUI uiText;
//
//     public void ResultCallback(string message)
//     {
//         Debug.Log("Your message is :" + message);
//         finalCallback?.Invoke(message);
//     }
//
//     public void PartialResultCallback(string message)
//     {
//         Debug.Log("Your partial result is :" + message);
//         partialCallback?.Invoke(message);
//     }
//
//     public static void StartListening(Action<string> callback)
//     {
//         // SpeechToText.Instance.onResultCallback = (string result) =>
//         // {
//         //     Debug.Log("Result from anonym " + result);
//         //     callback(result);
//         // };
//         // VoiceController.finalCallback = callback;
//         var flutterMessage = new FlutterMessage("listening", "start");
//         // var m1 = new Message { data = "{type:\"message\"},\"message\":\"listening start\"" };
//         // var m1 = new Message {data = JsonUtility.ToJson(flutterMessage)};
//         UnityMessageManager.Instance.SendMessageToFlutter(JsonUtility.ToJson(flutterMessage));
//         // Messages.Send(m1);
//         /*JsonUtility.ToJson(new SimilarityObjectMessage
//                 { similarity = similarity, type = "poseSimilarity" });*/
//         
//         
//         VoiceController.partialCallback = callback;
//         SpeechToText.Instance.StartRecording();
//     }
//
//     public static void StopListening()
//     {
//         var flutterMessage = new FlutterMessage("listening", "stop");
//         UnityMessageManager.Instance.SendMessageToFlutter(JsonUtility.ToJson(flutterMessage));
//         // var m1 = new Message { data = JsonUtility.ToJson(flutterMessage)};
//         // var m1 = new Message { data = "listening stop" };
//         // Messages.Send(m1);
//         SpeechToText.Instance.StopRecording();
//     }
//
//     private void Start()
//     {
//         SpeechToText.Instance.Setting(LANG_CODE);
//         
//         /*if (WorkoutStarter.localeCode == "zh")
//         {
//             SpeechToText.Instance.Setting(LANG_CODE_ZH);
//         }
//         else
//         {
//             SpeechToText.Instance.Setting(LANG_CODE);
//         }*/
//     }
// }