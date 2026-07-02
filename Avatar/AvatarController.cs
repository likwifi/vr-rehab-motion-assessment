using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using InteractiveModeScripts;
using Mediapipe.Unity.Tutorial;
using TMPro;
using UnityEngine;
using utils;
using Random = UnityEngine.Random;

public class AvatarController : MonoBehaviour
{
    private Vector3 avatarPos = new Vector3(0.4600f, 0, -0.4499f);

    private Vector3 dummyPose = new Vector3(-0.4799f, 0, -0.7199f);
    private string[] avatars = new[] { "male1", "female1", "male2", "female2", "male3", "male4", "female3", "female4","female5" };
    private string[] maleAvatars = new[] { "male1", "male2", "male3", "male4"  };
    private string[] femaleAvatars = new[] { "female1", "female2", "female3", "female4", "female5" };

    public static AvatarController Instance;

    private GameObject avatar;
    private GameObject avatarText;
    private GameObject dummyText;

    private GameObject dummy;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        var avatarName = WorkoutStarter.gender == "male"
            ? maleAvatars[Random.Range(0, maleAvatars.Length)]
            : femaleAvatars[Random.Range(0, femaleAvatars.Length)];

        // remove selected avatar so that dummy avatar is different
        var avatarsList = avatars.ToList();
        avatarsList.Remove(avatarName);
        avatars = avatarsList.ToArray();

        var dummyAvatarIndex = Random.Range(0, avatars.Length - 1);
        var avatarPrefab = Resources.Load<GameObject>(avatarName);
        var dummyPrefab = Resources.Load<GameObject>(avatars[dummyAvatarIndex]);
        var avatarTextPrefab = Resources.Load<GameObject>("AvatarName");

        var canvas = GameObject.Find("Canvas");
        if (avatarPrefab != null)
        {
            avatar = Instantiate(avatarPrefab, Vector3.zero, Quaternion.identity);
            avatar.transform.SetParent(transform);
            avatar.transform.localPosition = avatarPos;
            avatar.transform.localRotation = Quaternion.identity;
            avatar.transform.localScale = Vector3.one;
            avatar.GetComponent<Animator>().CrossFade("idle" + 3, 0);
            avatarText = Instantiate(avatarTextPrefab, Vector3.zero, Quaternion.identity);
            avatarText.transform.SetParent(canvas.transform);
            var myText = avatarText.GetComponent<TMP_Text>();
            myText.text = WorkoutStarter.userName ?? "";
        }

        if (dummyPrefab != null)
        {
            dummy = Instantiate(dummyPrefab, Vector3.zero, Quaternion.identity);
            dummy.transform.SetParent(transform);
            dummy.transform.localPosition = dummyPose;
            dummy.transform.localRotation = Quaternion.identity;
            dummy.transform.localScale = Vector3.one;
            StartIdle(dummy);
            dummyText = Instantiate(avatarTextPrefab, Vector3.zero, Quaternion.identity);
            dummyText.transform.SetParent(canvas.transform);
            var myText = dummyText.GetComponent<TMP_Text>();
            var isFemale = avatars[dummyAvatarIndex].StartsWith("female");
            myText.text = NameGenerator.GetRandomName(isFemale);
            FlutterMessage.SendMessage(myText.text, "dummyName");
        }

        MatchScales();
    }

    private Bounds GetHeight(GameObject cmp)
    {
        Renderer[] renderers = cmp.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("No Renderer components found in children.");
        }

        Bounds totalBounds = renderers[0].bounds;

        foreach (Renderer rend in renderers)
        {
            totalBounds.Encapsulate(rend.bounds);
        }

        return totalBounds;
    }

    private void MatchScales()
    {
        var character = GameObject.Find("Character");
        var b1 = GetHeight(avatar);
        var b2 = GetHeight(character);
        var scale = b2.size.y / b1.size.y;
        scale *= 1.05f;
        Debug.Log(b2.size.y / b1.size.y);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    bool dummyStarted = false;
    bool avatarStarted = false;
    private string currentDummyRepState = "";
    private string currentAvatarRepState = "";
    private Coroutine avatarCoroutine; // Store the running coroutine


    public void UpdateAvatar(RepState repState)
    {
        var animator = avatar.GetComponent<Animator>();

        if (!avatarStarted)
        {
            animator.CrossFade(repState.stateName, 0.2f);
            // avatarStarted = true;
            if (avatarCoroutine != null)
            {
                StopCoroutine(avatarCoroutine);
            }
        }

        avatarCoroutine = StartCoroutine(TransitionAfterDuration(animator, repState.totalDuration+0.2f));
    }


    public void UpdateDummy(RepState repState)
    {
        // Debug.Log(repState.stateName);
        if (currentDummyRepState != repState.stateName)
        {
            StopAllCoroutines();
            dummyStarted = false;
            currentDummyRepState = repState.stateName;
        }
        else if (!dummyStarted)
        {
            var trainerInteraction = InteractiveCharacterController.Instance.currentInteraction;

            var count = trainerInteraction.ObjectsCount;
            var totalTime = repState.totalDuration * count;
            var animator = dummy.GetComponent<Animator>();
            // Random value between 30% and 70% of count
            var dummyCount = Random.Range((int)Math.Round(count * 0.3), (int)Math.Round(count * 0.7));

            // animator.CrossFade(repState.stateName, 0);
            // StartCoroutine(TransitionAfterDuration(animator, repState.totalDuration));
            StartCoroutine(RunDummyCount(animator, totalTime, dummyCount, repState.stateName, repState.totalDuration));
            dummyStarted = true;
        }
    }

    private void Update()
    {
        // Update the UI text position
        avatarText.transform.position = Camera.main.WorldToScreenPoint(avatar.transform.position);
        dummyText.transform.position = Camera.main.WorldToScreenPoint(dummy.transform.position);
    }

    private IEnumerator RunDummyCount(Animator anim, float totalTime, int dummyCount, string stateName, float repTime)
    {
        var intervals = new float[dummyCount];
        var sum = 0f;
        for (var i = 0; i < dummyCount; i++)
        {
            intervals[i] = Random.Range(0.5f, 1.5f);
            sum += intervals[i];
        }

        // Scale each random interval so that their total equals totalTime
        for (var i = 0; i < dummyCount; i++)
        {
            intervals[i] = intervals[i] / sum * totalTime;
        }
    }
}
