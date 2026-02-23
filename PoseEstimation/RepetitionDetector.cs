using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

/*namespace Mediapipe.Unity.Tutorial
{
    public class RepetitionDetector
    {
        private List<float> primaryPositions;
        private List<float> secondaryPositions;
        private int repetitionCount;
        private bool isMovingPositive;
        private bool isMovingNegative;

        private const float minMovement = 0.1f; // Minimum movement to consider as a valid repetition
        private const int windowSize = 15; // Size of the window to smooth the data

        public enum Axis { X, Y, Z }

        private Axis movementAxis;

        public RepetitionDetector(Axis axis)
        {
            primaryPositions = new List<float>();
            secondaryPositions = null; // Initialize as null to indicate optional usage
            repetitionCount = 0;
            isMovingPositive = false;
            isMovingNegative = false;
            movementAxis = axis;
        }

        public int GetRepetitionCount()
        {
            return repetitionCount;
        }

        public void Update(NormalizedLandmark primaryJoint, NormalizedLandmark secondaryJoint = null)
        {
            // Capture the current position of the primary joint based on the selected axis
            float currentPrimaryPosition = GetPositionByAxis(primaryJoint, movementAxis);
            primaryPositions.Add(currentPrimaryPosition);

            // Optionally capture the current position of the secondary joint based on the selected axis
            if (secondaryJoint != null)
            {
                if (secondaryPositions == null)
                {
                    secondaryPositions = new List<float>();
                }
                float currentSecondaryPosition = GetPositionByAxis(secondaryJoint, movementAxis);
                secondaryPositions.Add(currentSecondaryPosition);

                // Keep the list size within the window size
                if (secondaryPositions.Count > windowSize)
                {
                    secondaryPositions.RemoveAt(0);
                }
            }

            // Keep the list size within the window size
            if (primaryPositions.Count > windowSize)
            {
                primaryPositions.RemoveAt(0);
            }

            // Calculate the average position within the window
            float averagePrimaryPosition = 0;
            foreach (float pos in primaryPositions)
            {
                averagePrimaryPosition += pos;
            }
            averagePrimaryPosition /= primaryPositions.Count;

            float averageSecondaryPosition = 0;
            if (secondaryPositions != null)
            {
                foreach (float pos in secondaryPositions)
                {
                    averageSecondaryPosition += pos;
                }
                averageSecondaryPosition /= secondaryPositions.Count;
            }

            // Determine if the joints are moving in the positive or negative direction along the selected axis
            if (primaryPositions.Count > 1 && (secondaryPositions == null || secondaryPositions.Count > 1))
            {
                bool isPrimaryMovingNegative = averagePrimaryPosition > currentPrimaryPosition + minMovement;
                bool isPrimaryMovingPositive = averagePrimaryPosition < currentPrimaryPosition - minMovement;
                bool isSecondaryMovingNegative = secondaryPositions == null || averageSecondaryPosition > GetPositionByAxis(secondaryJoint, movementAxis) + minMovement;
                bool isSecondaryMovingPositive = secondaryPositions == null || averageSecondaryPosition < GetPositionByAxis(secondaryJoint, movementAxis) - minMovement;

                // Debug.Log($"Primary - Avg: {averagePrimaryPosition}, Current: {currentPrimaryPosition}, Moving Negative: {isPrimaryMovingNegative}, Moving Positive: {isPrimaryMovingPositive}");
                if (secondaryJoint != null)
                {
                    // Debug.Log($"Secondary - Avg: {averageSecondaryPosition}, Current: {GetPositionByAxis(secondaryJoint, movementAxis)}, Moving Negative: {isSecondaryMovingNegative}, Moving Positive: {isSecondaryMovingPositive}");
                }

                if (isPrimaryMovingNegative && isSecondaryMovingNegative)
                {
                    isMovingNegative = true;
                    if (isMovingPositive)
                    {
                        // Count a repetition when both joints move negative after moving positive
                        repetitionCount++;
                        isMovingPositive = false;
                        Debug.Log($"Repetition Count: {repetitionCount}");
                    }
                }
                else if (isPrimaryMovingPositive && isSecondaryMovingPositive)
                {
                    isMovingPositive = true;
                    if (isMovingNegative)
                    {
                        isMovingNegative = false;
                    }
                }
            }
        }

        private float GetPositionByAxis(NormalizedLandmark joint, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return joint.X;
                case Axis.Y:
                    return joint.Y;
                case Axis.Z:
                    return joint.Z;
                default:
                    return joint.Y; // Default to Y if axis is not specified
            }
        }
    }
}*/

// only Y
namespace Mediapipe.Unity.Tutorial
{
   public class RepetitionDetector
   {
       public enum Axis { X, Y, Z }

       private Axis movementAxis;
       private List<float> primaryPositions;
       private List<float> secondaryPositions;
       private int repetitionCount;
       private bool isMovingUp;
       private bool isMovingDown;
       private List<float> thresholds { get; set; }

       private const float minVerticalMovement = 0.1f; // Minimum movement to consider as a valid repetition
       private const int windowSize = 30; // Size of the window to smooth the data

       public RepetitionDetector()
       {
           primaryPositions = new List<float>();
           secondaryPositions = null; // Initialize as null to indicate optional usage
           movementAxis = Axis.Y;
           repetitionCount = 0;
           isMovingUp = false;
           isMovingDown = false;
       }

       public int GetRepetitionCount()
       {
           return repetitionCount;
       }
       private float GetPositionByAxis(NormalizedLandmark joint)
       {
           switch (movementAxis)
           {
               case Axis.X:
                   return joint.X;
               case Axis.Y:
                   return joint.Y;
               case Axis.Z:
                   return joint.Z;
               default:
                   return joint.Y; // Default to Y if axis is not specified
           }
       }
       public void SetThresholds(List<float> _thresholds)
       {
           thresholds = _thresholds;
       }
    }
}
