using System.Collections;
using System.Collections.Generic;
using InteractiveModeScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StandaloneBundleLoader : MonoBehaviour
{
    [SerializeField] private GameObject URLInput;
    [SerializeField] private GameObject LessonURLInput;
    [SerializeField] private GameObject LoadButton;
    [SerializeField] private API api;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = LoadButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
}
