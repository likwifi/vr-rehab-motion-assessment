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

    private void TaskOnClick()
    {
        var url = URLInput.GetComponent<TMP_InputField>().text;
        var lessonUrl = LessonURLInput.GetComponent<TMP_InputField>().text;
        
        Debug.Log("Clicked");
        
        GameObject loadedScene = new GameObject("LoadedScene");
        
        api.GetBundleObject(url, (a, b) =>
        {
            if (lessonUrl is "" or "Lesson URL") return;
            var character = GameObject.Find("Character").GetComponent<InteractiveCharacterController>();
            StartCoroutine(character.requestWorkoutJSON(lessonUrl));

        }, loadedScene.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
