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

        public void StartOrbit(float totalDuration)
        {
            monoBehaviour.StartCoroutine(ProceedOrbitCamera(totalDuration));
        }

        private IEnumerator ProceedOrbitCamera(float totalDuration)
        {
            if (orbit == null)
                yield break;
            var panDuration = 12;
            var startTime = (totalDuration - panDuration) / 2;

            yield return new WaitForSeconds(startTime);

            var brain = mainCamera.GetComponent<CinemachineBrain>();
            brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);


            var start = initialRotation + new Vector3(0f, -25f, 0f);
            var end = initialRotation + new Vector3(0f, 25f, 0);
            var duration = 10f;
            monoBehaviour.StartCoroutine(VirtualCameraLerp(orbit, start, end, duration));
            yield return new WaitForSeconds(2);
            // nextMove("");

            yield return new WaitForSeconds(duration + 2);
            orbit.transform.rotation = Quaternion.Euler(initialRotation);
            brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        }

        private IEnumerator VirtualCameraLerp(GameObject camera, Vector3 start, Vector3 end,
            float lerpDuration)
        {
            float timeElapsed = 0;

            while (timeElapsed < lerpDuration)
            {
                camera.transform.rotation =
                    Quaternion.Lerp(Quaternion.Euler(start.x, start.y, start.z),
                        Quaternion.Euler(end.x, end.y, end.z),
                        timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}