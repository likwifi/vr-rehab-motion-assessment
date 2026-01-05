#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Google.Protobuf.Collections;
using Mediapipe.Unity.CoordinateSystem;
using Newtonsoft.Json;
using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.Recorder.Examples;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
    public class PoseRecorder : MonoBehaviour
    {
        [SerializeField] private TextAsset _configAssetGPU;
        [SerializeField] private TextAsset _configAssetCPU;
        [SerializeField] private RawImage _screen;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _fps;
        [SerializeField] private MovieRecorder movieRecorder;

        private CalculatorGraph _graph;
        private static ResourceManager _resourceManager;


        private WebCamTexture _webCamTexture;
        private Texture2D _inputTexture;
        private Color32[] _inputPixelData;
        private Texture2D _outputTexture;
        private Color32[] _outputPixelData;

        private bool _partialBodyNotificationSend = false;
        private bool _bodyFullyVisibleNotifcationSend = false;

        private void Awake()
        {
            if (!ResourceManager.isInitialized)
                _resourceManager = new StreamingAssetsResourceManager();
        }

        private IEnumerator Start()
        {

            var inferenceMode = InferenceMode.GPU;
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            Logger.LogWarning("Current platform does not support GPU inference mode, so falling back to CPU mode");
            Debug.Log("Using GPU");
            inferenceMode = InferenceMode.CPU;
#else
        Debug.Log("Using GPU");
        if (!GpuManager.IsInitialized)
                  yield return GpuManager.Initialize();
#endif

            if (WebCamTexture.devices.Length == 0)
            {
                throw new System.Exception("Web Camera devices are not found");
            }

            var webCamDevice = WebCamTexture.devices[inferenceMode.Equals(InferenceMode.GPU) ? 1 : 0];
            _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
            _webCamTexture.Play();

            yield return new WaitUntil(() => _webCamTexture.width > 16);


            _screen.rectTransform.sizeDelta = new Vector2(_width, _height);

            _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            _inputPixelData = new Color32[_width * _height];
            _outputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            _outputPixelData = new Color32[_width * _height];

            _screen.texture = _outputTexture;

            yield return _resourceManager.PrepareAssetAsync("pose_detection.bytes");
            yield return _resourceManager.PrepareAssetAsync("pose_landmark_lite.bytes");

            var stopwatch = new Stopwatch();

            if (inferenceMode.Equals(InferenceMode.GPU))
            {
                _graph = new CalculatorGraph(_configAssetGPU.text);
                _graph.SetGpuResources(GpuManager.GpuResources).AssertOk();
            }
            else
            {
                _graph = new CalculatorGraph(_configAssetCPU.text);
            }

            var outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(_graph, "output_video");
            var poseLandmarksStream =
                new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(_graph, "pose_landmarks");

            outputVideoStream.StartPolling().AssertOk();
            poseLandmarksStream.StartPolling().AssertOk();

            _graph.StartRun().AssertOk();
            stopwatch.Start();

            var screenRect = _screen.GetComponent<RectTransform>().rect;
            var poseGraph = new PoseGraph();
            while (true)
            {
                var visibleLandmarks = new List<int>();
                _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
                var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, _width, _height, _width * 4,
                    _inputTexture.GetRawTextureData<byte>());
                var currentTimestamp = stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000);

                _graph.AddPacketToInputStream("input_video",
                        new ImageFramePacket(imageFrame, new Timestamp(currentTimestamp)))
                    .AssertOk();

                yield return new WaitForSeconds((1f / _fps));

                if (outputVideoStream.TryGetNext(out var outputVideo))
                {
                    if (outputVideo.TryReadPixelData(_outputPixelData))
                    {
                        _outputTexture.SetPixels32(_outputPixelData);
                        _outputTexture.Apply();
                    }
                }

                if (poseLandmarksStream.TryGetNext(out var poseLandmarks))
                {
                    if (poseLandmarks != null)
                    {
                        int i = 0;
                        foreach (var landmark in poseLandmarks.Landmark)
                        {
                            if (landmark.Presence > 0.8)
                            {
                                visibleLandmarks.Add(i);
                            }

                            i++;
                            // Debug.Log($"{landmark.X},{landmark.Y}, {landmark.Z}");
                            // Debug.Log($"Unity Local Coordinates: {screenRect.GetPoint(topOfHead)}, Image Coordinates: {topOfHead}");
                        }
                        
                        if (!recordPressed) continue;
                        // StartCoroutine(SpeechUtils.GetAudioClip("Recording Now!",0f, KidsCharacterController._characterAudioSource,null,null));
                        Debug.Log("Recording...");
                        var angles = poseGraph.calculateAngels(poseLandmarks.Landmark);
                        Record(poseLandmarks.Landmark, angles, exerciseName, _fps);
                    }
                }
            }
        }
    }
}
