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

    private IEnumerator EnemyGenerator(InteractionObject interaction)
    {
        var numberOfObjects = interaction.ObjectsCount;
        var stateName = interaction.StateName;
        var generatorObject = interaction.GeneratorObject;
        if (generatorObject != "")
        {
            if (block == null)
            {
                block = (GameObject)Resources.Load(generatorObject, typeof(GameObject));
            }
        }

        for (var i = 0; i < numberOfObjects; i++)
        {
            var targetPosition = target.transform.position;
            var cubePosition = targetPosition * 2.1f;
            var rotation = _character.transform.rotation;
            var characterRotation = rotation.y;

            // var cubeRotation = Quaternion.LookRotation(target.transform.position - _character.transform.position);
            //* (Mathf.PI)
            var angle = /*Random.Range(0f, numberOfObjects) * */
                Random.Range((characterRotation - 60), (characterRotation + 60)) /*/
                numberOfObjects*/;
            var x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            var z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            var pos = targetPosition + new Vector3(x, 0, z);
            var angleDegrees = -angle;
            // var rot = Quaternion.Euler(cubeRotation.x, cubeRotation.y, cubeRotation.z);
            // cubeRotation.x = 0;
            var objRotation = new Quaternion(rotation.x,  - rotation.y,
                rotation.z,1f);
            var a1 = Instantiate(block, pos, objRotation);
            var o = gameObject;
            a1.transform.parent = o.transform;
            _enemyBlocks.Add(a1);
            yield return new WaitForSeconds(2);
        }

        _character.SendMessage("NextMove");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var b in _enemyBlocks)
        {
            var position1 = b.transform.position;
            var step = MoveSpeed * Time.deltaTime; // calculate distance to move
            b.transform.position = Vector3.MoveTowards(position1, target.transform.position, step);
        }
    }
}