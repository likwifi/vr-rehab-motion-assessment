using System;
using UnityEngine;

namespace Mediapipe.Unity.Tutorial
{
    [Serializable]
    public class PoseVector3
    {
        // [SerializeField]
        public float[] _c = new float[3]; 
        public PoseVector3(float x, float y, float z)
        {
            _c[0] = x;
            _c[1] = y;
            _c[2] = z;

        }
        // public float[] _c { get; set; }
        
        public Vector3 ToVec()
        {
            return new Vector3(_c[0], _c[1], _c[2]);
        }
    }
}