using System;
using System.Collections;
using System.Collections.Generic;
using InteractiveModeScripts;
using UnityEngine;
using utils;

public class InteractiveWorkoutLoader : MonoBehaviour
{
    private InteractiveCharacterController character;
    private IEnumerator helperPromptCoroutine;


    private Dictionary<string, string> localeBundle = LocaleBundle.get();

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character").GetComponent<InteractiveCharacterController>();
        helperPromptCoroutine = helperPrompt();
        StartCoroutine(startWorkot());
    }

    private IEnumerator startWorkot()
    {
        character.SetState("greet");
        character.setSpeaker("Tony");
        character.setSpeakerStyle("Friendly");
        yield return character.speak(string.Format(localeBundle["workoutTypeQuestion"], WorkoutStarter.userName), null,
            (object text) =>
            {
                character.StartListening(VoiceCallback);
                StartCoroutine(helperPromptCoroutine);
            });
    }
}
