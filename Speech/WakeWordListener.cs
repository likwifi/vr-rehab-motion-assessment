// using System;
// using System.Collections;
// using System.Collections.Generic;
// using FlutterUnityPlugin;
// using InteractiveModeScripts;
// using UnityEngine;
// using utils;
//
// public class WakeWordListener : MonoBehaviour
// {
//
//     public GameObject character;
//     
//     private GameObject siriObj;
//     private InteractiveCharacterController characterController;
//     private bool initialized;
//     void Start()
//     {
//         characterController = character.GetComponent<InteractiveCharacterController>();
//     }
//     
//     void Update()
//     {
//         if (InteractiveCharacterController.workoutFinished && !initialized)
//         {
//             StartListening();
//             initialized = true;
//         }
//     }
//
//     private void StartListening()
//     {
//         Debug.Log("Started Listening");
//         var siri =  (GameObject) Resources.Load("SiriObj", typeof(GameObject));
//         // render at place where no geometry exists.
//         var loc = new Vector3(-1111.0270000007f, 1111.1170001f, -1111.17600012f);
//         siriObj = Instantiate(siri, loc, transform.rotation);
//         void Resume(string type)
//         {
//             StartCoroutine(characterController.speak(LocaleBundle.get()["gettingReady"], null, (object text) =>
//             {
//                 var flutterMessage = new FlutterMessage("find", type);
//                 // var m1 = new Message { data = "{type:\"message\"},\"message\":\"listening start\"" };
//                 var m1 = new Message {data = JsonUtility.ToJson(flutterMessage)};
//                 Messages.Send(m1);
//                 
//                 // StartResumeWorkout();
//                 // continueWorkoutCallback();
//             }));
//             Destroy(siriObj);
//             VoiceController.StopListening();
//         }
//         
//         void CallBack(string result)
//         {
//             var resultStr = (string)result;
//             Debug.Log("Voice result is " + resultStr);
//             var types= new []{"yoga", "stregth", "hiit", "kids", "high intensity"};
//             foreach (var t in types)
//             {
//                 if (resultStr.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
//                 {
//                     Resume(t);
//                 }
//             }
//         }
//         
//         VoiceController.StartListening(CallBack);
//     }
// }
