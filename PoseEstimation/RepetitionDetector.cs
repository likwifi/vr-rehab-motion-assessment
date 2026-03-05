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
       public void SetMovementAxis([CanBeNull] string axis)
       {
           if (Enum.TryParse(axis, out Axis axisEnum))
           {
               movementAxis = axisEnum;
           }
           else
           {
               Debug.LogError("Failed to parse enum");
           }
       }

       public void CleanUp()
       {
           primaryPositions = new List<float>();
           secondaryPositions = null;
           repetitionCount = 0;
       }

       public void Update(NormalizedLandmark primaryJoint, NormalizedLandmark secondaryJoint = null)
       {
           // if (thresholds != null)
           // {
            // SetThresholds(thresholds);
           // }

           Debug.Log(thresholds);
           // Capture the current Y position of the primary joint
           float currentPrimary = GetPositionByAxis(primaryJoint);
           primaryPositions.Add(currentPrimary);

           // Optionally capture the current Y position of the secondary joint
           if (secondaryJoint != null)
           {
               if (secondaryPositions == null)
               {
                   secondaryPositions = new List<float>();
               }

               float currentSecondaryY = GetPositionByAxis(secondaryJoint);
               secondaryPositions.Add(currentSecondaryY);

               // Keep the list size within the window size
               if (secondaryPositions.Count > windowSize)
               {
                   secondaryPositions.RemoveAt(0);
               }
           }
           else
           {
               secondaryPositions = null;
           }

           // Keep the list size within the window size
           if (primaryPositions.Count > windowSize)
           {
               primaryPositions.RemoveAt(0);
           }

           // Calculate the average position within the window
           float averagePrimaryMove = 0;
           foreach (float movement in primaryPositions)
           {
               averagePrimaryMove += movement;
           }

           averagePrimaryMove /= primaryPositions.Count;

           float averageSecondaryMove = 0;
           if (secondaryPositions != null)
           {
               foreach (float movement in secondaryPositions)
               {
                   averageSecondaryMove += movement;
               }

               averageSecondaryMove /= secondaryPositions.Count;
           }

           // Determine if the joints are moving up or down
           if (primaryPositions.Count > 1 && (secondaryPositions == null || secondaryPositions.Count > 1))
           {
               bool isPrimaryMovingDown = averagePrimaryMove > GetPositionByAxis(primaryJoint) + GetThreshold(0) ;
               bool isPrimaryMovingUp = averagePrimaryMove < GetPositionByAxis(primaryJoint) - GetThreshold(0);
               bool isSecondaryMovingDown = secondaryPositions == null ||
                                            averageSecondaryMove > GetPositionByAxis(secondaryJoint) + GetThreshold(1);
               bool isSecondaryMovingUp = secondaryPositions == null ||
                                          averageSecondaryMove < GetPositionByAxis(secondaryJoint) - GetThreshold(1);

               if (isPrimaryMovingDown && isSecondaryMovingDown)
               {
                   isMovingDown = true;
                   if (isMovingUp)
                   {
                       // Count a repetition when both joints move down after moving up
                       repetitionCount++;
                       isMovingUp = false;
                   }
               }
               else if (isPrimaryMovingUp && isSecondaryMovingUp)
               {
                   isMovingUp = true;
                   if (isMovingDown)
                   {
                       isMovingDown = false;
                   }
               }
           }
       }

       private double GetThreshold(int i)
       {
           if (thresholds.ElementAt(i) != null)
           {
               return thresholds.ElementAt(i);
           }

           return minVerticalMovement;
       }
    }
}
