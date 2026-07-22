using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlanarReflection : MonoBehaviour
{
    private Vector2 Resolution;

    private Camera ReflectionCamera;
    private CinemachineBrain brain;
    private CinemachineVirtualCamera activeCam;
    [SerializeField] private float distance;
    [SerializeField] private float rotationDelta;

    [SerializeField] private RenderTexture ReflectionRenderTexture;
    [SerializeField] private int ReflectionResloution;

    public void Start()
    {
        StartCoroutine(SetCameraObjects());
    }

    private IEnumerator SetCameraObjects()
    {
        yield return new WaitUntil(() => GameObject.Find("Main Camera").GetComponent<CinemachineBrain>() != null);
        var tt = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        activeCam = (CinemachineVirtualCamera)tt.ActiveVirtualCamera;
        ReflectionCamera = GameObject.Find("ReflectionCamera").GetComponent<Camera>();
        ReflectionCamera.fieldOfView = activeCam.m_Lens.FieldOfView;

        /*ReflectionCamera.transform.position = new Vector3(activeCam.transform.position.x,
            activeCam.transform.position.y - distance, activeCam.transform.position.z);*/
    }
}
