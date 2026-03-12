namespace Mediapipe.Unity.Tutorial
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ActionRecognition : MonoBehaviour
    {
        // Define dimensions: 50 time steps, 17 joints, and 3 coordinates (x, y, z)
        private const int timeSteps = 50;
        private const int joints = 17;
        private const int coordinates = 3;

        // Lists to store the exercise templates and their corresponding labels
        private List<float[]> templates;
        private List<string> labels;

        // Start is called before the first frame update
        void Start()
        {
            // Initialize the lists
            templates = new List<float[]>();
            labels = new List<string>();

            // Generate 100 random exercise templates (each is a flattened representation of a 3D sequence)
            for (int i = 0; i < 100; i++)
            {
                float[] template = GenerateRandomSequence();
                templates.Add(template);
                labels.Add("exercise_" + i);
            }

            // Generate a new random sequence that we wish to classify
            float[] newSequence = GenerateRandomSequence();

            // Classify the new sequence using DTW-based matching against the templates
            string predictedClass = ClassifySequence(newSequence);
            Debug.Log("Predicted Exercise: " + predictedClass);
        }

        // Generates a random sequence representing a (50, 17, 3) action and flattens it into a 1D array.
        float[] GenerateRandomSequence()
        {
            int sequenceLength = timeSteps * joints * coordinates;
            float[] sequence = new float[sequenceLength];
            for (int i = 0; i < sequenceLength; i++)
            {
                sequence[i] = Random.value; // Random float between 0 and 1
            }

            return sequence;
        }
    }
}
