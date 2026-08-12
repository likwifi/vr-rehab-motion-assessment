using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    private GameObject character;

    private Vector3 touchStartPos;
    private Vector3 touchMovePos;
    private float lastMoveAngle = 0f;
    private int orientation = 1;
    private Vector3 lastTouchMovePos = Vector3.zero;
    private Quaternion initialCharacterRotation;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        initialCharacterRotation = transform.rotation;
        mainCamera = Camera.main;
        character = GameObject.Find("Character");
    }
}
