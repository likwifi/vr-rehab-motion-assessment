using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mediapipe.Unity.Tutorial
{
    [Serializable]
    public class PoseCompliance
    {
        public  int[] toleranceRate;
        public string name;
        public List<Keyframe> bagOfKeyframes;
        public string message;
        private int startFeature = 0;
        private bool[] complyValues;
        private Action<int> poseCompliedFeedback;
        private const int defaultTolerance = 10;
        public PoseCompliance(string name, List<Keyframe> bagOfKeyframes)
        {
            this.name = name;
            this.bagOfKeyframes = bagOfKeyframes;
            complyValues = new bool[bagOfKeyframes.Count];
            toleranceRate = new int[bagOfKeyframes.Count];
        }

        public void complies(int[][] angles)
        {
            
            for (var i = startFeature; i < bagOfKeyframes.Count; i++)
            {
                var frame = bagOfKeyframes[i];
                var featureComply = frame.comply(angles);
                complyValues[i] = featureComply;
                if (featureComply)
                {
                    if (i < bagOfKeyframes.Count - 1)
                    {
                        startFeature++;
                    }
                    else
                    {
                        // when last frame complied assume that movement is complete
                        poseComplied();
                    }
                }
                else
                {
                    toleranceRate[i]++;
                }

                if (toleranceRate[i] > defaultTolerance && !frame.messageSend)
                {
                    Debug.Log(frame.message);
                    frame.messageSend = true;
                    toleranceRate[i] = 0;
                }
            }

            
        }

        private void poseComplied()
        {
            startFeature = 0;
            var time = bagOfKeyframes[^1].complianceTime - bagOfKeyframes[0].complianceTime;
            poseCompliedFeedback?.Invoke(time);
        }

        public void setOnCompliedFeedback(Action<int> action)
        {
            poseCompliedFeedback = action;
        }
    }
    [Serializable]
    public class Keyframe
    {
        public int[][] featureAngles;
        public  int[][] thresholds;
        public string message;
        public bool messageSend = false;

        public int complianceTime = 0;

        public Keyframe(int[][]featureAngles,int[][]thresholds, string message = "")
        {
            this.message = message;
            this.featureAngles = featureAngles;
            this.thresholds = thresholds;
        }

        public bool comply(int[][] anglesToComply)
        {
            for (var i = 0; i < featureAngles.Length; i++)
            {
                var angles = featureAngles[i];
                if (angles == null) continue;
                for (var j = 0; j < angles.Length; j++)
                {
                    var complyValue = (angles[j] - anglesToComply[i][j]) < thresholds[i][j];
                    if (!complyValue)
                        return false;
                    // complyValues.Add(complyValue);
                }
            }

            complianceTime = Environment.TickCount; // milliseconds since system start
            return true;
        }
    }
}