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

/*
// zero crossing
namespace Mediapipe.Unity.Tutorial
{
    public class RepetitionDetector
    {
        private List<Vector3> jointPositions;
        private List<float> velocities;

        private int repetitionCount;
        private bool isMovingPositive;
        private bool isMovingNegative;

        private const float minMovement = 0.7f; // Minimum movement to consider as a valid repetition
        private const int windowSize = 100; // Size of the window to smooth the data

        public RepetitionDetector()
        {
            jointPositions = new List<Vector3>();
            velocities = new List<float>();
            repetitionCount = 0;
            isMovingPositive = false;
            isMovingNegative = false;
        }

        public int GetRepetitionCount()
        {
            return repetitionCount;
        }
        
        public void SetRepetitionCount(int count)
        {
            repetitionCount = count;
        }

        int fileCount = 0;
        string str = "x,y,z\n";
      
        List<double> CalculateVelocities(List<(double x, double y, double z)> jointPositions)
        {
            var velocities = new List<double>();

            for (int i = 1; i < jointPositions.Count; i++)
            {
                double velocity = Math.Sqrt(
                    Math.Pow(jointPositions[i].x - jointPositions[i - 1].x, 2) +
                    Math.Pow(jointPositions[i].y - jointPositions[i - 1].y, 2) +
                    Math.Pow(jointPositions[i].z - jointPositions[i - 1].z, 2)
                );

                velocities.Add(velocity);
            }

            return velocities;
        }

        int CountRepetitions(List<float> velocities)
        {
            int zeroCrossings = 0;

            for (int i = 1; i < velocities.Count; i++)
            {
                if ((velocities[i] > 0 && velocities[i - 1] <= 0) ||
                    (velocities[i] <= 0 && velocities[i - 1] > 0))
                {
                    zeroCrossings++;
                }
            }

            return zeroCrossings / 2; // Each repetition has two zero-crossings
        }
        double CustomRound(double value, double threshold)
        {
            if (Math.Abs(value) < threshold)
            {
                return 0;
            }
            return value;
        }
        public void Update(NormalizedLandmark joint)
        {
            // Capture the current position of the joint
            Vector3 currentJointPosition = new Vector3(joint.X, joint.Y, joint.Z);
/*#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            var filePath = Application.dataPath + "/jointPositions.txt";

            if (fileCount < 1000)
            {
                fileCount++;
                str += $"{joint.X}, {joint.Y}, {joint.Z}\n";
            }
            else
            {
                Debug.Log("flushed file");
                File.WriteAllText(filePath, str);
            }
#endif#1#
            jointPositions.Add(currentJointPosition);

            // Keep the list size within the window size
            if (jointPositions.Count > windowSize)
            {
                jointPositions.RemoveAt(0);
                velocities.RemoveAt(0);
                // Debug.Log(string.Join(", ", jointPositions.Select(d => d.ToString()).ToArray()));
            }
            
            // Calculate the velocity for the new position
            if (jointPositions.Count > 1)
            {
                int count = jointPositions.Count;
                /*var velocity =  Math.Sqrt(
                    Math.Pow(jointPositions[count - 1].x - jointPositions[count - 2].x, 2) +
                    Math.Pow(jointPositions[count - 1].y - jointPositions[count - 2].y, 2) +
                    Math.Pow(jointPositions[count - 1].z - jointPositions[count - 2].z, 2)
                );#1#
                var dx = (jointPositions[count - 1].x - jointPositions[count - 2].x);
                var dy = (jointPositions[count - 1].y - jointPositions[count - 2].y);
                // var dz = 100*(jointPositions[count - 1].z - jointPositions[count - 2].z);

                var distanceSquared = dx * dx + dy * dy;
                var velocity = Mathf.Sqrt(distanceSquared);
                // var velocity= Vector3.Distance(jointPositions[count - 1], jointPositions[count - 2]);

                // Check if velocity is effectively zero with a small tolerance
                
                
                if (Math.Abs(velocity) < 1e-4)
                {
                    velocity = 0;
                }
/*Vector3.Distance(jointPositions[count - 1], jointPositions[count - 2])#1#;
                // velocity = Mathf.Round((float)(velocity * 1000f)) / 1000f;
                // velocity = CustomRound(velocity, 0.0001);
                velocities.Add(velocity);
                if (jointPositions.Count > 90)
                {
                    // Debug.Log(string.Join(", ", jointPositions.Select(d => d.ToString()).ToArray()));
                    // Debug.Log(string.Join(", ", velocities.Select(d => d.ToString()).ToArray()));
                }

                
                // Count repetitions if there are enough data points
                if (velocities.Count > 1)
                {
                    var numRepetitions = CountRepetitions(velocities);
                    Debug.Log($"Dummy Count: {numRepetitions}");

                    if (numRepetitions > 0)
                    {
                        repetitionCount++;
                        Debug.Log($"Repetition Count: {repetitionCount}");
                    }
                }
            }
        }
    }
}*/




/*
// x,y,z one point
namespace Mediapipe.Unity.Tutorial
{
    public class RepetitionDetector
    {
        private List<Vector3> jointPositions;
        private int repetitionCount;
        private bool isMovingPositive;
        private bool isMovingNegative;

        private const float minMovement = 0.7f; // Minimum movement to consider as a valid repetition
        private const int windowSize = 100; // Size of the window to smooth the data

        public RepetitionDetector()
        {
            jointPositions = new List<Vector3>();
            repetitionCount = 0;
            isMovingPositive = false;
            isMovingNegative = false;
        }

        public int GetRepetitionCount()
        {
            return repetitionCount;
        }

        int fileCount = 0;
        string str = "x,y,z\n";
      
        public void Update(NormalizedLandmark joint)
        {
            // Capture the current position of the joint
            Vector3 currentJointPosition = new Vector3(joint.X, joint.Y, joint.Z);
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            var filePath = Application.dataPath + "/jointPositions.txt";

            if (fileCount < 1000)
            {
                fileCount++;
                str += $"{joint.X}, {joint.Y}, {joint.Z}\n";
            }
            else
            {
                Debug.Log("flushed file");
                File.WriteAllText(filePath, str);
            }
#endif
            jointPositions.Add(currentJointPosition);

            // Keep the list size within the window size
            if (jointPositions.Count > windowSize)
            {
                jointPositions.RemoveAt(0);
            }

            // Calculate the average position within the window
            var averageJointPosition = jointPositions.Aggregate(Vector3.zero, (current, pos) => current + pos);
            averageJointPosition /= jointPositions.Count;

            // Determine the axis with the largest movement
            var movementVector = currentJointPosition - averageJointPosition;
            var absMovementX = Mathf.Abs(movementVector.x);
            var absMovementY = Mathf.Abs(movementVector.y);
            var absMovementZ = Mathf.Abs(movementVector.z);

            var maxMovement = Mathf.Max(absMovementX, absMovementY, absMovementZ);

            // Determine if the joint is moving in the positive or negative direction along the most significant axis
            if (maxMovement == absMovementX)
            {
                isMovingNegative = movementVector.x > minMovement;
                isMovingPositive = movementVector.x < -minMovement;
            }
            else if (maxMovement == absMovementY)
            {
                isMovingNegative = movementVector.y > minMovement;
                isMovingPositive = movementVector.y < -minMovement;
            }
            else
            {
                isMovingNegative = movementVector.z > minMovement;
                isMovingPositive = movementVector.z < -minMovement;
            }

            Debug.Log($" Moving Negative: {isMovingNegative}, Moving Positive: {isMovingPositive}");
            // Debug.Log($"Current Position: {currentJointPosition}, Average Position: {averageJointPosition}, Movement Vector: {movementVector}, Max Movement: {maxMovement}");

            if (jointPositions.Count > 1)
            {
                if (isMovingNegative)
                {
                    this.isMovingNegative = true;
                    if (isMovingPositive)
                    {
                        // Count a repetition when the joint moves negative after moving positive
                        repetitionCount++;
                        isMovingPositive = false;
                        Debug.Log($"Repetition Count: {repetitionCount}");
                    }
                }
                else if (isMovingPositive)
                {
                    this.isMovingPositive = true;
                    if (this.isMovingNegative)
                    {
                        this.isMovingNegative = false;
                    }
                }
            }
        }
    }
}*/

/*
public class RepetitionDetector
{
    public NormalizedLandmark shoulderRight;
    public NormalizedLandmark elbowRight;

    private List<float> elbowYPositions;
    private int repetitionCount;
    private bool isMovingUp;
    private bool isMovingDown;

    private const float minVerticalMovement = 0.1f; // Minimum movement to consider as a valid repetition
    private const int windowSize = 90; // Size of the window to smooth the data

    public RepetitionDetector()
    {
        elbowYPositions = new List<float>();
        repetitionCount = 0;
        isMovingUp = false;
        isMovingDown = false;
    }
    

    public void Update()
    {
        // Capture the current Y position of the elbow
        float currentElbowY = elbowRight.Y;
        elbowYPositions.Add(currentElbowY);

        // Keep the list size within the window size
        if (elbowYPositions.Count > windowSize)
        {
            elbowYPositions.RemoveAt(0);
        }

        // Calculate the average elbow position within the window
        float averageElbowY = 0;
        foreach (float y in elbowYPositions)
        {
            averageElbowY += y;
        }
        averageElbowY /= elbowYPositions.Count;

        // Determine if the elbow is moving up or down
        if (elbowYPositions.Count > 1)
        {
            if (averageElbowY > elbowRight.Y + minVerticalMovement)
            {
                isMovingDown = true;
                if (isMovingUp)
                {
                    // Count a repetition when the elbow moves down after moving up
                    repetitionCount++;
                    isMovingUp = false;
                }
            }
            else if (averageElbowY < elbowRight.Y - minVerticalMovement)
            {
                isMovingUp = true;
                if (isMovingDown)
                {
                    isMovingDown = false;
                }
            }
        }

        // Output the repetition count (for debugging purposes)
        Debug.Log("Repetition Count: " + repetitionCount);
    }
}
}*/