using System.Collections;
using System.Collections.Generic;
using InteractiveModeScripts;
using InteractiveModeScripts.obj;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractiveObjectGenerator : MonoBehaviour
{
    private GameObject _character;
    public GameObject target;
    public GameObject block;
    public float radius = 2f;

    public float MoveSpeed = 0.8f; //move speed

    private const float RotationSpeed = 2; //speed of turning

    private List<GameObject> _enemyBlocks = new List<GameObject>();

// public static string 
    // Start is called before the first frame update
    void Start()
    {
        _character = GameObject.Find("Character");
        // StartCoroutine(nameof(EnemyGenerator));
    }

    void StartGenerator(InteractionObject interaction)
    {
        StartCoroutine(EnemyGenerator(interaction));
    }
}
