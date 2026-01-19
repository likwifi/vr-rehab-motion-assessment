#if UNITY_EDITOR
using System;
using Mediapipe.Unity.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecordButtonHandler : MonoBehaviour
{
    private TextMeshProUGUI _textCmp;
    public GameObject textGameObject;
    public PoseRecorder recorder;
    void Start()
    {
        // AddComponent(PoseRecorder());
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }

    void Update()
    {
        //Check for mouse click 
        /*if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                   //Our custom method. raycastHit.transform.gameObject
                   StartStopRecording();
                }
            }
        }*/
    }

    private void Awake()
    {
        _textCmp = textGameObject.GetComponent<TextMeshProUGUI>();
        GetComponent<Image>().color = UnityEngine.Color.white;
    }

    public void StartStopRecording()
    {
        recorder.recordPressed = !recorder.recordPressed;
        recorder.StartStopRecording();
        _textCmp.text = recorder.recordPressed ? "Stop Recording." : "Start Recording.";
        GetComponent<Image>().color = recorder.recordPressed ? UnityEngine.Color.red : UnityEngine.Color.white;
    }
}
#endif