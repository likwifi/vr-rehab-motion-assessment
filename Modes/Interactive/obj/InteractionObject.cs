using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace InteractiveModeScripts.obj
{
    [Serializable]
    public class InteractionObject
    {
        public string AnnounceText;
        public int ObjectsCount;
        public string StateName;
        public string GeneratorObject;

        public bool Repeating;
        public float TotalDuration;
        public bool backToIdle;
        public string BackToIdleState;
        public string ParallelAnnounceText;
        public float ParallelAnnounceDelay;
        public float  BackToIdleStateDuration;
        public float ExerciseStartDelay;
        
        public string IdleState;
        public Vector3 cameraPosition;

        public string camera;
        public Quaternion cameraRotation;

        // TODO add speaker voice
        public InteractionObject(string stateName, int objectsCount, string announceText, string ParallelAnnounceText, float totalDuration = 0f,
            bool repeating = false, bool backToIdle = true,Vector3 cameraPosition=default,Quaternion cameraRotation=default, string IdleState = "Idle Pose",
            string BackToIdleState = "", float BackToIdleStateDuration = 0f, float ExerciseStartDelay = 0f, string GeneratorObject= "", string camera = "")
        {
            StateName = stateName;
            ObjectsCount = objectsCount;
            AnnounceText = announceText;
            TotalDuration = totalDuration;
            Repeating = repeating;
            this.backToIdle = backToIdle;
            this.cameraPosition = cameraPosition;
            this.IdleState = IdleState;
            this.BackToIdleState = BackToIdleState;
            this.BackToIdleStateDuration = BackToIdleStateDuration;
            this.ParallelAnnounceText = ParallelAnnounceText;
            this.ExerciseStartDelay = ExerciseStartDelay;
            this.cameraRotation = cameraRotation;
            this.ParallelAnnounceDelay = ParallelAnnounceDelay;
            this.GeneratorObject = GeneratorObject;
            this.camera = camera;
        }
    }
}