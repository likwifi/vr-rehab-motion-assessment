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
    }
}
