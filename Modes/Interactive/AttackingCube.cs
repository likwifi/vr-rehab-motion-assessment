using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InteractiveModeScripts;
using UnityEngine;

public class AttackingCube : MonoBehaviour
{
    // public Animator animator;
    private GameObject _explosion;
    private InteractiveCharacterController _character;

    private void Start()
    {
        // PurpleBlast
        // Explosion Galaxy
        _explosion = (GameObject) Resources.Load("PurpleBlast", typeof(GameObject));
        _character = GameObject.Find("Character").GetComponent<InteractiveCharacterController>();
    }
}
