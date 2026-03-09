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

        void Update()
        {
            // Get accelerometer data
            Vector3 acceleration = Input.acceleration;
            float magnitude = Mathf.Sqrt(acceleration.x * acceleration.x + acceleration.y * acceleration.y + acceleration.z * acceleration.z);

            // Apply low-pass filter
            float filteredValue = lastFilteredValue + alpha * (magnitude - lastFilteredValue);
            lastFilteredValue = filteredValue;
            filteredMagnitudes.Add(filteredValue);

            // Peak detection
            timeSinceLastPeak += Time.deltaTime;
            if (filteredMagnitudes.Count > 1)
            {
                int lastIndex = filteredMagnitudes.Count - 1;
                if (filteredMagnitudes[lastIndex] > peakThreshold && filteredMagnitudes[lastIndex] > filteredMagnitudes[lastIndex - 1] && timeSinceLastPeak >= peakDistance)
                {
                    repetitionCount++;
                    timeSinceLastPeak = 0.0f;
                    Debug.Log("Repetition Count: " + repetitionCount);
                }
            }
        }
    }
}