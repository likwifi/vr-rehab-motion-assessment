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

    private void OnTriggerEnter(Collider other)
    {
        _character.SendMessage("OnInteraction");
        
        StartCoroutine(DestroyCube(other.transform.position));
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        _character.SendMessage("OnInteraction");
        StartCoroutine(DestroyCube(collision.contacts[0].point));
    }*/

    private IEnumerator DestroyCube(Vector3 transformPosition)
    {
        // yield return new WaitForSeconds(0.4f);
        // yield return new WaitUntil(()=>_character.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        yield return new WaitForSeconds(_character.getAnimator().GetCurrentAnimatorClipInfo(0).Length*0.9f);
        
        var explosion = Instantiate(_explosion, transform.position, transform.rotation);
        Destroy(gameObject);
        yield return new WaitForSeconds(1);
        Destroy(explosion);
        
        // gameObject.SetActive(false);
    }
}