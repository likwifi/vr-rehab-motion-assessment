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

        // Compares a new sequence to all stored templates using DTW and returns the label of the best-matching template.
        string ClassifySequence(float[] sequence)
        {
            float bestDistance = float.PositiveInfinity;
            int bestMatch = -1;

            // Iterate over each template to compute the DTW distance
            for (int i = 0; i < templates.Count; i++)
            {
                float distance = DTWDistance(sequence, templates[i]);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestMatch = i;
                }
            }

            // Return the label associated with the template that has the minimum DTW distance
            return labels[bestMatch];
        }

        // Implements the Dynamic Time Warping (DTW) algorithm to compute the distance between two sequences.
        float DTWDistance(float[] s, float[] t)
        {
            int n = s.Length;
            int m = t.Length;
            float[,] dtw = new float[n + 1, m + 1];

            // Initialize the DTW matrix with positive infinity
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    dtw[i, j] = float.PositiveInfinity;
                }
            }

            dtw[0, 0] = 0;

            // Populate the DTW matrix with cumulative costs
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute the local cost (absolute difference between the corresponding elements)
                    float cost = Mathf.Abs(s[i - 1] - t[j - 1]);
                    // Update the DTW cost matrix entry using the minimum of the three neighboring cells
                    dtw[i, j] = cost + Mathf.Min(dtw[i - 1, j], Mathf.Min(dtw[i, j - 1], dtw[i - 1, j - 1]));
                }
            }

            // The DTW distance between the two sequences is the cost in the bottom-right cell of the matrix
            return dtw[n, m];
        }
    }
}