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

    // Update is called once per frame
    Touch touch;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("Began");

                    if (Camera.main != null)
                        touchStartPos = touch.position;

                    lastTouchMovePos = touch.position;
                    // touchStartPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    touchMovePos = touch.position;
                    var diff = lastTouchMovePos - touchMovePos;
                    orientation = diff.x >= 0 ? 1 : -1;
                    // var angle = Vector3.Angle(lastTouchMovePos, touchMovePos);
                    /*if (lastMoveAngle == 0f)
                    {
                        lastMoveAngle = angle;
                    }
                    else if ((angle-lastMoveAngle) < 0)
                    {
                        orientation = -1;
                    }*/
                    Debug.Log(diff.x);

                    if (initialCharacterRotation.x - 180 > transform.rotation.x &&
                        initialCharacterRotation.x + 180 < transform.rotation.x)
                    {
                        Debug.Log("In Range");
                    }

                    // transform.position = new Vector3(transform.position.x + touch.deltaPosition.x, transform.position.y, transform.position.z + touch.deltaPosition.y);
                    if (Mathf.Abs(diff.x) > 7)
                        transform.RotateAround(character.transform.position, Vector3.up,
                            0.3f * orientation * diff.magnitude);
                    // lastMoveAngle = angle;
                    lastTouchMovePos = touchMovePos;
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Ended");
                    break;
            }
        }
    }
}