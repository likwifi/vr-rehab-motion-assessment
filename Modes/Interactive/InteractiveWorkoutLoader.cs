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

    private IEnumerator helperPrompt()
    {
        yield return new WaitForSeconds(8);
        character.StopListening();
        yield return character.speak(localeBundle["trainerHelperPrompt"], null,
            (object text) => { character.StartListening(VoiceCallback); });
    }

    private void VoiceCallback(string result)
    {
        var resultStr = result;

        Debug.Log(resultStr);

        var hiitWords = new List<string>
            { localeBundle["hiit"], localeBundle["strength"], localeBundle["core"], localeBundle["hiitW"] };
        var martialWords = new List<string> { localeBundle["martial"], localeBundle["boxing"], localeBundle["kick"] };
        var yoga = localeBundle["yoga"];
        var kids = localeBundle["kids"];
        foreach (var hiitWord in hiitWords)
        {
            if (resultStr.IndexOf(hiitWord, StringComparison.OrdinalIgnoreCase) > -1)
            {
                WorkoutFound("HIIT");
                return;
            }
        }

        foreach (var word in martialWords)
        {
            if (resultStr.IndexOf(word, StringComparison.OrdinalIgnoreCase) > -1)
            {
                WorkoutFound("MARTIAL");
                return;
            }
        }

        if (resultStr.IndexOf(yoga, StringComparison.OrdinalIgnoreCase) > -1)
        {
            WorkoutFound("YOGA");
            return;
        }
        else if (resultStr.IndexOf(kids, StringComparison.OrdinalIgnoreCase) > -1)
        {
            WorkoutFound("KIDS");
            return;
        }
    }

    private void WorkoutFound(string type)
    {
        StopCoroutine(helperPromptCoroutine);
        character.StopListening();
        character.setSpeaker("Tony");
        character.setSpeakerStyle("Friendly");
        StartCoroutine(character.speak(string.Format(localeBundle["workoutFound"], type), null,
            (object text) => { FlutterMessage.SendMessage("FIND_" + type); }));
    }
}