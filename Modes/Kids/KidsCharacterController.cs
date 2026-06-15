// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Lessons.Hatsune;
// using obj;
// using TMPro;
// using UnityEngine;
// using utils;
//
// public class KidsCharacterController : CharacterControllerBase
// {
//     public static AudioSource _characterAudioSource;
//     private int countNumber = 0;
//     private int totalCounts = 0;
//
//     public static KidsCharacterController instance;
//     private static Animator m_Animator;
//     private static MovesList<MoveObj> movements;
//     private static int MoveIndex = 0;
//     private static bool MovementsPaused = false;
//
//     private TextMeshProUGUI counter;
//     private GameObject ProgressBarGameObject;
//     public TextAsset movesJSON;
//     public static TextAsset correctMovesJSON;
//     private CircleFillHandler progressBar;
//     public void Start()
//     {
//         if (movesJSON != null)
//         {
//             MovementsPaused = false;
//             movements = SaveManager.Load(movesJSON.ToString()).moves;
//             // StartCoroutine(nameof(WorkoutRunner), movements);
//             instance.StartCoroutine(nameof(WorkoutRunner), movements);
//             // TODO
//             // movements = SaveManager.Load(movesJSON.ToString());
//             // StartCoroutine(nameof(WorkoutRunner), movements);
//         }
//
//         if (correctMovesJSON != null)
//         {
//         }
//     }
//
//     // Start is called before the first frame update
//     private void Awake()
//     {
//         // ProgressBarGameObject = GameObject.Find("circleFillHandler");
//         _characterAudioSource = GetComponent<AudioSource>();
//         m_Animator = gameObject.GetComponent<Animator>();
//
//         // counter = GameObject.Find("Counter").GetComponent<TextMeshProUGUI>();
//         // progressBar = ProgressBarGameObject.GetComponent<CircleFillHandler>();
//         instance = this;
//     }
//
//     private void OnEnable()
//     {
//         Debug.Log("Enabled");
//     }
//
//     private static void Resume()
//     {
//         MovementsPaused = false;
//         // movements = SaveManager.Load(movesJSON);
//         // StartCoroutine(nameof(WorkoutRunner), movements);
//         instance.StartCoroutine(nameof(WorkoutRunner), movements);
//     }
//
//     public static void Pause()
//     {
//         MovementsPaused = true;
//         m_Animator.CrossFade("Happy Idle", 0.5f);
//     }
//
//     public static void BodyFullyVisible()
//     {
//         string userName = "John";
//         string message = "I can see you now, " + userName + ", lets continue.";
//
//         instance.StartCoroutine(SpeechUtils.GetAudioClip(message,
//             0f,
//             _characterAudioSource, null, (object text) => { Resume(); }));
//         VoiceController.StopListening();
//     }
//
//     public static void BodyNotFullyVisible(Action continueWorkoutCallback)
//     {
//         string userName = "John";
//         string message = "Sorry " + userName + ", Can't see your full body." +
//                          "Please place yourself in such a way that your body is fully visible " +
//                          "or say continue, to proceed without body movement analysis.";
//         Pause();
//         instance.StartCoroutine(SpeechUtils.GetAudioClip(message,
//             0f,
//             _characterAudioSource, null, (object text) => { ListenForCallback(continueWorkoutCallback); }));
//     }
//
//
//     private static void ListenForCallback(Action continueWorkoutCallback)
//     {
//         void CallBack(string result)
//         {
//             string resultStr = (string)result;
//             Debug.Log("Voice result is " + resultStr);
//             string positiveWord = "continue";
//             if (resultStr.IndexOf(positiveWord, StringComparison.OrdinalIgnoreCase) > -1)
//             {
//                 instance.StartCoroutine(SpeechUtils.GetAudioClip("Resuming workout.",
//                     0f,
//                     _characterAudioSource, null, (object text) =>
//                     {
//                         Resume();
//                         continueWorkoutCallback();
//                     }));
//                 VoiceController.StopListening();
//             }
//         }
//
//         Debug.Log("Started Listening");
//         VoiceController.StartListening(CallBack);
//
// #if UNITY_EDITOR
//         // instance.StartCoroutine(SpeechUtils.GetAudioClip("Resuming workout.",
//         //     0f,
//         //     _characterAudioSource, null, (object text) =>
//         //     {
//         //         Resume();
//         //         continueWorkoutCallback();
//         //     }));
// #endif
//     }
//
//
//
//
//     private IEnumerator WorkoutRunner()
//     {
//         if (movements == null)
//             yield break;
//         for (; MoveIndex < movements.moves.Count; MoveIndex++)
//         {
//             if (MovementsPaused)
//                 yield break;
//             MoveObj moveObj = movements.moves[MoveIndex];
//             if (moveObj.BeforeAnnounceAnimationName != null)
//                 m_Animator.CrossFade(moveObj.BeforeAnnounceAnimationName, moveObj.TransitionDuration);
//
//             var duration = moveObj.AfterAnnounceAnimationTime;
//             var totalDuration = duration * moveObj.TotalRepeats;
//             totalCounts = moveObj.TotalRepeats;
//
//             void CallbackOnFinish(object text)
//             {
//                 if (moveObj.AfterAnnounceAnimationName != null && !MovementsPaused)
//                 {
//                     m_Animator.CrossFade(moveObj.AfterAnnounceAnimationName, 0.5f);
//                     InvokeRepeating(nameof(UpdateCounter), 0.5f, duration);
//                     StartCoroutine(ExecuteAfterTime(totalDuration, () => { ResetCounter(); }));
//                 }
//             }
//             yield return GetAudioClipService(moveObj.AnnounceText, null, (text) =>
//             {
//                 if (moveObj.AfterAnnounceAnimationName != null)
//                 {
//                     m_Animator.CrossFade(moveObj.AfterAnnounceAnimationName, 0.5f);
//                     StartCounter(duration, totalDuration);
//                     // InvokeRepeating(nameof(UpdateCounter), 0.5f, duration);
//                     // StartCoroutine(ExecuteAfterTime(totalDuration, () => { ResetCounter(); }));
//                 }
//                 StartCoroutine(ExecuteAfterTime(totalDuration, NextMove));
//             });
//
//             /*yield return SpeechUtils.GetAudioClip(moveObj.AnnounceText, totalDuration, _characterAudioSource, null,
//                 CallbackOnFinish);*/
//         }
//     }
//
//     private bool isCoroutineExecuting;
//
//     IEnumerator ExecuteAfterTime(float time, Action task)
//     {
//         if (isCoroutineExecuting)
//             yield break;
//         isCoroutineExecuting = true;
//         yield return new WaitForSeconds(time);
//         task();
//         isCoroutineExecuting = false;
//     }
//
//     private void UpdateCounter()
//     {
//         return;
//         countNumber += 1;
//         counter.text = countNumber.ToString();
//         progressBar.fillValue = (100 / totalCounts) * countNumber;
//     }
//
//     private void ResetCounter()
//     {
//         return;
//         totalCounts = 0;
//
//         progressBar.fillValue = 0;
//         CancelInvoke();
//         countNumber = 0;
//         counter.text = countNumber.ToString();
//     }
// }