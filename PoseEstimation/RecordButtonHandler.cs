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
}
