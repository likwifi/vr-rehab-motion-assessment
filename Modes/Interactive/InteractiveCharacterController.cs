using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CartoonFX;
using Cinemachine;
using FlutterUnityIntegration;
using InteractiveModeScripts.obj;
using JetBrains.Annotations;
using Mediapipe.Unity.Tutorial;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using utils;
using Random = System.Random;

namespace InteractiveModeScripts
{
    // public TextAsset movesJSON;
    public class InteractiveCharacterController : CharacterControllerBase
    {
        // public TextAsset movesJSON;
        [CanBeNull] public GameObject generator;
        public static InteractiveCharacterController Instance;

        [CanBeNull] public TextAsset workoutJson;

        [CanBeNull] public ParticleSystem trail;
        [SerializeField] public Color victoryBg;

        // Orange Victory #EC7029
        private string interactionName;
        private Vector3 defaultMainCameraPosition;
        private Quaternion defaultMainCameraRotation;

        public static bool workoutFinished = false;

        // private MovesList<InteractionObject> moves;
        private bool generatorExists;
        private string lastState = "";
        private string clipText = "";

        public string WorkoutType = "";
        private Dictionary<string, GameObject> _cameras;

        private Dictionary<string, CinemachineVirtualCamera> _virtualCameras;
        private OrbitCamera orbitCamera;

        // Start is called before the first frame update
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            workoutFinished = false;
            generatorExists = generator != null;
            MovementsPaused = false;

            mainCamera = GameObject.Find("Main Camera");
            defaultMainCameraPosition = mainCamera.transform.localPosition;
            defaultMainCameraRotation = mainCamera.transform.localRotation;
            isInitialized = true;
            InteractionWorkoutObj workout = null;
            if (workoutJson != null)
            {
                workout = InteractionSaveManger.Load(workoutJson.ToString());
            }

            InitCameras();
            InitVirtualCameras();
            if (_cameras["Orbit"] != null && WorkoutType.Equals("hiit"))
            {
                orbitCamera = new OrbitCamera(_cameras["Orbit"], mainCamera, this);
            }

            InitWorkout(workout);
            
            
#if UNITY_EDITOR
            var animator = GetComponent<Animator>();
            if (animator.runtimeAnimatorController == null)
            {
                animator.runtimeAnimatorController = Resources.Load("HIIT_All") as RuntimeAnimatorController;
            }

            // StartCoroutine(ShowVFXDummy("a"));

            // speakUIMessage("It's important to know your age to determine the best fitness plan for you and your family");
            /*
            ControllerToWorkoutJson.CollectClipCurves(m_Animator);
            ControllerToWorkoutJson.CollectAnimatorStatesInfo(m_Animator);*/
#endif
        }

        private void CreateAvatars()
        {
            var parent = transform.parent;

            var sibling = new GameObject("Avatars");
            sibling.transform.SetParent(parent);
            sibling.transform.localPosition = transform.localPosition;
            sibling.transform.rotation = transform.localRotation;
            sibling.transform.localScale = Vector3.one;

            sibling.AddComponent<AvatarController>();
        }

        void Update()
        {
            // pinch to zoom camera
            if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved &&
                Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var camera = _cameras["Medium"];
                if (camera != null)
                    StartCoroutine(CameraLerp(camera.transform.localPosition, camera.transform.localRotation));
                /*curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
                prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
                touchDelta = curDist.magnitude - prevDist.magnitude;
                speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
                speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

                if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {

                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView + (1 * speed),15,90);
                }

                if ((touchDelta +varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {

                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - (1 * speed),15,90);
                }*/
            }
        }

        private void InitCameras()
        {
            _cameras = new Dictionary<string, GameObject>
            {
                { "Wide", GameObject.Find("Wide") },
                { "Medium", GameObject.Find("Medium") },
                { "Left", GameObject.Find("Left") },
                { "Right", GameObject.Find("Right") },
                { "Orbit", GameObject.Find("Orbit") }
            };
        }

        private void InitVirtualCameras()
        {
            var wide = GameObject.Find("Wide");
            var hasWide = wide != null;
            _virtualCameras = new Dictionary<string, CinemachineVirtualCamera>
            {
                { "Wide", hasWide ? wide.GetComponent<CinemachineVirtualCamera>() : null },
                { "Medium", hasWide ? GameObject.Find("Medium").GetComponent<CinemachineVirtualCamera>() : null },
            };
            var centerCamera = GameObject.Find("Center");
            if (centerCamera != null)
            {
                _virtualCameras["Center"] = centerCamera.GetComponent<CinemachineVirtualCamera>();
            }
        }

        private void SwitchVirtualCamera(CinemachineVirtualCamera virtualCamera)
        {
            foreach (var cinemachineVirtualCamera in _virtualCameras.Values)
            {
                cinemachineVirtualCamera.Priority = cinemachineVirtualCamera == virtualCamera ? 10 : 0;
            }
        }

        /* Start of Calls from flutter */
        public void setWorkoutJson(string workoutUrl)
        {
            // Debug.Log("Workout URL is ");
            // Debug.Log(workoutUrl);
            StartCoroutine(requestWorkoutJSON(workoutUrl));
        }

        public void speakUIMessageParallel(string message)
        {
            StartCoroutine(speak(message));
        }

        public void ShowVFX(string data)
        {
            var vfxID = data.Split("_")[0];
            var text = data.Split("_")[1];
            var vfxName = vfxID switch
            {
                "1" => "pop",
                "2" => "pop_text",
                "3" => "boom",
                "4" => "electric",
                "5" => "hit_blue",
                "6" => "hit_yellow",
                "7" => "slash_yellow",
                "8" => "poof",
                _ => "pop"
            };

            StartCoroutine(ProceedShowVFX(vfxName, text));
        }

        private IEnumerator ShowVFXDummy(string vfxID)
        {
            yield return new WaitForSeconds(3);

            StartCoroutine(ProceedShowVFX("pop", "Wuah"));
            yield return new WaitForSeconds(3);
            StartCoroutine(ProceedShowVFX("pop_text", "janeee"));
            yield return new WaitForSeconds(3);
            StartCoroutine(ProceedShowVFX("hit_yellow", "gnaac"));
        }

        private IEnumerator ProceedShowVFX(string vfxName, string text)
        {
            var vfxPrefab = (GameObject)Resources.Load("vfx/" + vfxName, typeof(GameObject));
            var vfx = Instantiate(vfxPrefab, Vector3.zero, Quaternion.identity).transform;
            vfx.parent = mainCamera.transform;
            vfx.localPosition = new Vector3(0, 0, 2.5f);
            vfx.localRotation = Quaternion.identity;
            vfx.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            var particleText = vfx.GetComponentInChildren<CFXR_ParticleText>();
            particleText.UpdateText(text);
            yield return new WaitForSeconds(3);
            Destroy(vfx.gameObject);
        }

        public void speakUIMessage(string message)
        {
            StartCoroutine(speak(message, (text) =>
                {
                    var r = new Random();
                    var rInt = r.Next(0, 4);
                    Debug.Log("neutral_talk" + rInt);
                    CrossFadeMove("neutral_talk" + rInt, 0.3f);
                    StartCoroutine(SwitchToMedium());
                },
                (text) =>
                {
                    Debug.Log("Idle");
                    CrossFadeMove("Idle Pose", 0.3f);
                    FlutterMessage.SendMessage("Speak_Idle");
                    SwitchVirtualCamera(_virtualCameras["Wide"]);
                }, false));
        }

        public void SetWorkout(string workoutString)
        {
            /*

            animator.runtimeAnimatorController = Resources.Load("HIIT_All") as RuntimeAnimatorController;
            */

            // Debug.Log("Workout URL is ");
            // Debug.Log(workoutUrl);
            var animator = GetComponent<Animator>();
            var workout = JObject.Parse(workoutString);
            WorkoutType = (string)workout.GetValue("type");
            var lessonJSON = (string)workout.GetValue("lessonJSON");

            if (WorkoutType != null && animator.runtimeAnimatorController == null)
            {
                switch (WorkoutType)
                {
                    case "hiit":
                    case "kids":
                        animator.runtimeAnimatorController = Resources.Load("HIIT_All") as RuntimeAnimatorController;
                        break;
                    case "yoga":
                        animator.runtimeAnimatorController = Resources.Load("Yoga_all") as RuntimeAnimatorController;
                        break;
                    case "martial":
                        animator.runtimeAnimatorController =
                            Resources.Load("Kickboxing_all") as RuntimeAnimatorController;
                        break;
                }
            }

            if (lessonJSON != null)
            {
                StartCoroutine(requestWorkoutJSON(lessonJSON));
            }
        }

        public void nextMove(string obj)
        {
            // Debug.Log("Workout URL is ");
            // Debug.Log(workoutUrl);
            NextMoveFromController();
        }

        public void skipPoseAnalysis(string obj)
        {
            PoseEstimation.Hide();
            StartResumeWorkout();
        }

        public void hideControls(string obj)
        {
            ProgressBarGameObject1.SetActive(!obj.Equals("true"));
        }

        /* End of Calls from flutter */

        private void InitWorkout(InteractionWorkoutObj workout)
        {
            if (workout != null)
            {
                // BLOCKER
                if (WorkoutType.Equals("hiit"))
                {
                    CreateAvatars();
                }

                if (!string.IsNullOrEmpty(workout.speaker))
                {
                    _speaker = workout.speaker;
                    _speakerStyle = workout.speakerStyle;
                }

                if (!string.IsNullOrEmpty(workout.localeCode))
                {
                    localeCode = workout.localeCode;
                }

                moves = workout.moves;
                Debug.Log(moves == null ? "Moves Empty FROM INIT" : "Moves SET!!!!");

                timerType = workout.timerType;
                if (!MovementsPaused)
                    StartCoroutine(nameof(PreWorkoutRunner));
            }
            else
            {
                // StartCoroutine(requestWorkoutJSON("https://firebasestorage.googleapis.com/v0/b/optical-net-91011.appspot.com/o/classes%2FHana%2Fjson%2Flesson1.json?alt=media&token=e39ff666-03eb-4cea-b3a8-7975f9b4cef1"));
            }
        }

        private void NextMove()
        {
            moveIndex++;
            // assuming first move is greeting message
            if (moveIndex == 1)
            {
                FlutterMessage.SendMessage("WORKOUT_STARTED");
                if (ProgressBarGameObject1 != null)
                    ProgressBarGameObject1.SetActive(true);
            }

            FlutterMessage.SendMessage((moves.Count - 1).ToString(), "workoutMovesCount");
            FlutterMessage.SendMessage(moveIndex.ToString(), "workoutMoveIndex");

            workoutFinished = moveIndex == moves.Count - 1;
            if (moveIndex < moves.Count)
            {
                StartCoroutine(nameof(WorkoutRunner), moves[moveIndex]);
            }
        }
    }
}
