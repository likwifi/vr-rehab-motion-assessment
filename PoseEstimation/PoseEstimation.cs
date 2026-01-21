using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using InteractiveModeScripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using utils;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Mediapipe.Unity.Tutorial
{
    public class PoseEstimation : MonoBehaviour
    {
        [SerializeField] private TextAsset _configAssetGPU;
        [SerializeField] private TextAsset _configAssetCPU;
        [SerializeField] private TextAsset _correctWorkout;
        [SerializeField] private RawImage _screen;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _fps;

        private CharacterControllerBase character;
        private List<RepState> repStates;

        private CalculatorGraph _graph;
        private static ResourceManager _resourceManager;

        private WebCamTexture _webCamTexture;
        private Texture2D _inputTexture;
        private Color32[] _inputPixelData;
        private Texture2D _outputTexture;
        private Color32[] _outputPixelData;
        public RepState repExercise;
        private bool _partialBodyNotificationSend = false;
        private bool _bodyFullyVisibleNotificationSend = false;

        public enum ModelComplexity
        {
            Lite = 0,
            Full = 1,
            Heavy = 2,
        }

        private void Awake()
        {
            if (!ResourceManager.isInitialized)
                _resourceManager = new StreamingAssetsResourceManager();
            var repJointsText = Resources.Load<TextAsset>("rep_joints");
            if (repJointsText != null)
            {
                // Deserialize the JSON data
                repStates = JsonConvert.DeserializeObject<List<RepState>>(repJointsText.text);
                FlutterMessage.SendMessage("Reps JSON loaded", "metadata");
            }
            else
            {
                Debug.LogError("Could not find the JSON file!");
            }

            // Invoke(nameof(UpdateAvatar), 28.0f);
        }

        private IEnumerator Start()
        {
            yield return _resourceManager.PrepareAssetAsync("pose_detection.bytes");
            yield return _resourceManager.PrepareAssetAsync("pose_landmark_lite.bytes");
            if (WebCamTexture.devices.Length == 0)
            {
                throw new Exception("Web Camera devices are not found");
            }

            var inferenceMode = InferenceMode.GPU;

#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            Logger.LogWarning("Current platform does not support GPU inference mode, so falling back to CPU mode");
            Debug.Log("Using CPU");
            inferenceMode = InferenceMode.CPU;
#else
        Debug.Log("Using GPU");
        FlutterMessage.SendMessage("Using GPU", "metadata");
        if (!GpuManager.IsInitialized)
            yield return GpuManager.Initialize();
#endif
            var webCamDevice = WebCamTexture.devices[inferenceMode.Equals(InferenceMode.GPU) ? 1 : 0];
            // BLOCKER
            _fps = 12;
            _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
            _webCamTexture.Play();

            yield return new WaitUntil(() => _webCamTexture.width > 16);
            yield return new WaitUntil(() => CharacterControllerBase.isInitialized);
            getCharacter();
            character.setPoseEsimtator(this);
            var stopwatch = new Stopwatch();

            try
            {
                _screen.rectTransform.sizeDelta = new Vector2(_width, _height);

                _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
                _inputPixelData = new Color32[_width * _height];

#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
                _outputTexture = new Texture2D(320, 240, TextureFormat.RGBA32, false);
                _outputPixelData = new Color32[320 * 240];
                /*_outputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
                _outputPixelData = new Color32[_width * _height];*/
#else
            /*_outputTexture = new Texture2D(320, 240, TextureFormat.RGBA32, false);
            _outputPixelData = new Color32[320 * 240];*/
            _outputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            _outputPixelData = new Color32[_width * _height];
#endif
                FlutterMessage.SendMessage(_width + "," + _height, "metadata");

                _screen.texture = _outputTexture;

                if (inferenceMode.Equals(InferenceMode.GPU))
                {
                    _graph = new CalculatorGraph(_configAssetGPU.text);
                    _graph.SetGpuResources(GpuManager.GpuResources).AssertOk();
                }
                else
                {
                    _graph = new CalculatorGraph(_configAssetCPU.text);
                }
            }
            catch (Exception e)
            {
                FlutterMessage.SendMessage(e.ToString(), "metadata");
                Debug.Log(e.ToString());
            }

            var outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(_graph, "output_video");
            var poseLandmarksStream =
                new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(_graph, "pose_landmarks");

            outputVideoStream.StartPolling().AssertOk();
            poseLandmarksStream.StartPolling().AssertOk();

            var sidePacket = new SidePacket();
            sidePacket.Emplace("model_complexity", new IntPacket((int)ModelComplexity.Lite));
            _graph.StartRun(sidePacket).AssertOk();
            stopwatch.Start();

            // var screenRect = _screen.GetComponent<RectTransform>().rect;


#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            // StartCoroutine(ExerciseTester());
#endif
            while (true)
            {
                try
                {
                    _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
                    var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, _width, _height, _width * 4,
                        _inputTexture.GetRawTextureData<byte>());
                    var currentTimestamp = stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);

                    _graph.AddPacketToInputStream("input_video",
                            new ImageFramePacket(imageFrame, new Timestamp(currentTimestamp)))
                        .AssertOk();
                    if (outputVideoStream.TryGetNext(out var outputVideo))
                    {
                        // FlutterMessage.SendMessage(outputVideo.Width() + "_out_" + outputVideo.Height(), "metadata");
                        // FlutterMessage.SendMessage(_outputPixelData.Length + "_out_length", "metadata");

                        if (outputVideo.TryReadPixelData(_outputPixelData))
                        {
                            _outputTexture.SetPixels32(_outputPixelData);
                            _outputTexture.Apply();
                            // FlutterMessage.SendMessage("SettedPixels", "metadata");
                        }
                    }
                }
                catch (Exception e)
                {
                    FlutterMessage.SendMessage(e.ToString(), "metadata");
                    Debug.Log(e.ToString());
                }

                CountReps(poseLandmarksStream);

                /*try
                {
                    CountReps(poseLandmarksStream);
                }
                catch (Exception e)
                {
                    FlutterMessage.SendMessage(e.ToString(), "metadata");
                    Debug.Log(e.ToString());
                }*/

                yield return new WaitForSeconds((1f / _fps));
            }
        }

        String previousState = "";
        RepState previousExercise = null;
        readonly RepetitionDetector detector = new();

        private void CountReps(OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> poseLandmarksStream)
        {
            var visibleLandmarks = new List<int>();

            // reps count
            var currentState = character.currentAnimatorState;

            if (previousState != currentState)
            {
                repExercise = repStates.Find(r => r.stateName == currentState);
                previousState = currentState;
                detector.CleanUp();
                FlutterMessage.SendMessage(0.ToString(), "repsCount");

                Debug.Log("Cleanup");
            }

            if (!Equals(repExercise, previousExercise))
            {
                previousExercise = repExercise;
                if (repExercise != null)
                {
                    LowerValuesByPercentage(repExercise.thresholds, 0.1f);
                    Debug.Log("Lowered");
                }
            }

            if (!poseLandmarksStream.TryGetNext(out var poseLandmarks)) return;
            if (poseLandmarks == null) return;
            var i = 0;
            foreach (var landmark in poseLandmarks.Landmark)
            {
                if (landmark.Presence > 0.8)
                {
                    visibleLandmarks.Add(i);
                }

                i++;
            }

            if (visibleLandmarks.Count < 26) // 33 all
            {
                if (!_partialBodyNotificationSend)
                {
                    _partialBodyNotificationSend = true;
                    BodyNotFullyVisible();
                }
            }
            else if (!_bodyFullyVisibleNotificationSend)
            {
                _bodyFullyVisibleNotificationSend = true;
                StartCoroutine(BodyFullyVisible());
            }

            if (repExercise == null) return;

            detector.SetThresholds(repExercise.thresholds);
            if (repExercise.axis != null)
            {
                detector.SetMovementAxis(repExercise.axis);
            }

            var oldReps = detector.GetRepetitionCount();
            var repJoints = repExercise.joints;
            var joint1 = poseLandmarks.Landmark[repJoints[0]];
            var joint2 = repJoints.Count > 1 ? poseLandmarks.Landmark[repJoints[1]] : null;
            AvatarController.Instance.UpdateDummy(repExercise);

            if (joint1.Presence > 0.8)
            {
                detector.Update(joint1, joint2 != null && joint2.Presence > 0.8 ? joint2 : null);
                var newReps = detector.GetRepetitionCount();
                if (newReps != oldReps)
                {
                    FlutterMessage.SendMessage(newReps.ToString(), "repsCount");
                    AvatarController.Instance.UpdateAvatar(repExercise);

                    Debug.Log(newReps.ToString());
                }
            }
            // Debug.Log(curresentState + " count: " + detector.GetRepetitionCount());
        }



        private void LowerValuesByPercentage(List<float> floatList, float percentage)
        {
            for (int i = 0; i < floatList.Count; i++)
            {
                floatList[i] -= floatList[i] * percentage;
            }
        }

        private int exerciseTesterIndex = 52;
        // private int exerciseTesterIndex = 28;

        private IEnumerator ExerciseTester()
        {
            repExercise = repStates[exerciseTesterIndex];
            character.SetState(repExercise.stateName);
            // repExercise = repStates.Find(r => r.stateName == repExercise.stateName);

            // Debug.Log(repExercise.StateName);
            yield return new WaitForSeconds(150);
            exerciseTesterIndex++;
            StartCoroutine(ExerciseTester());
        }

        private IEnumerator fakeTip()
        {
            // TODO temp solution for demo purposes.
            yield return new WaitForSeconds(22f);
            yield return character.speak(
                "Lower your body until your thighs are parallel to the ground.");
            yield return new WaitForSeconds(10f);
            yield return character.speak(
                "Excellent work!");
        }

        private void getCharacter()
        {
            character = GameObject.Find("Character").GetComponent<CharacterControllerBase>();
        }


        private IEnumerator BodyFullyVisible()
        {
            yield return new WaitForSeconds(3f);

            character.BodyFullyVisible();
            // KidsCharacterController.BodyFullyVisible();
        }
    }
}
