using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace InteractiveModeScripts
{
    public class OrbitCamera
    {
        private readonly GameObject orbit;
        private readonly GameObject mainCamera;
        private Vector3 initialRotation;
        private readonly MonoBehaviour monoBehaviour;

        public OrbitCamera(GameObject orbit, GameObject mainCamera, MonoBehaviour monoBehaviour)
        {
            this.orbit = orbit;
            this.monoBehaviour = monoBehaviour;
            this.mainCamera = mainCamera;
            initialRotation = orbit.transform.localEulerAngles;
        }

        public void ResetOrbit()
        {
            var brain = mainCamera.GetComponent<CinemachineBrain>();
            if (initialRotation != null)
            {
                orbit.transform.rotation = Quaternion.Euler(initialRotation);
            }

            brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        }
    }
}
