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

        public void StartPoseAnalysis(RepeatedField<NormalizedLandmark> poseLandmarks, long time)
        {
            // foreach (var t in _exercises)
            // {
            //     if (t.start < time && t.end > time)
            //     {
            //         _currentExercise = t;
            //         break;
            //     }
            // }

            AddMove(poseLandmarks);
            float[] similarity = _currentExercise.CalculatePoseSimilarity(_moves);
            similarityList.Add(similarity); // TODO 
            var text = GameObject.Find("Correlation").GetComponent<TextMeshProUGUI>();
            var totalSimilarity = similarity.Aggregate(0.0, (current, t) => current + t);

            text.text = totalSimilarity.ToString(CultureInfo.InvariantCulture);
            UnityMessageManager.Instance.SendMessageToFlutter(
                JsonUtility.ToJson(new SimilarityObjectMessage
                { similarity = similarity, type = "poseSimilarity" }));
        }

        class SimilarityObjectMessage
        {
            public float[] similarity;
            public string type;
        }
    }
}
