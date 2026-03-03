using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Tutorial
{
    public class ExerciseRepetitionCounter : MonoBehaviour
    {
        public float cutoffFrequency = 3.0f;
        public float sampleRate = 50.0f;
        public float peakThreshold = 0.5f;
        public float peakDistance = 0.2f; // Minimum time between peaks in seconds

        private float nyquistFrequency;
        private float alpha;
        private float lastFilteredValue;
        private float timeSinceLastPeak;
        private int repetitionCount;

        private List<float> filteredMagnitudes = new List<float>();

        void Start()
        {
            nyquistFrequency = 0.5f * sampleRate;
            alpha = cutoffFrequency / nyquistFrequency;
            lastFilteredValue = 0.0f;
            timeSinceLastPeak = 0.0f;
            repetitionCount = 0;
        }
    }
}
