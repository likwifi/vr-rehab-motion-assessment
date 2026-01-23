using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FlutterUnityIntegration;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
// using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace Mediapipe.Unity.Tutorial
{
    public class PoseAnalyzer
    {
        private List<Exercise> _exercises;

        private readonly List<Vector3[]> _moves = new();
        private List<float[]> similarityList = new();
        private readonly Exercise _currentExercise;

        public PoseAnalyzer(string json)
        {
            _currentExercise = JsonConvert.DeserializeObject<Exercise>(json);
        }
    }
}
