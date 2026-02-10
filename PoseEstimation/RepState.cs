using System.Collections.Generic;

namespace Mediapipe.Unity.Tutorial
{
    public class RepState
    {
        public string stateName { get; set; }
        public float totalDuration { get; set; }

        public List<int> joints { get; set; }
        public List<float> thresholds { get; set; }
        public string axis { get; set; }
        
    }
}