using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    float randomOffset;

    public string AnimationName;
    public int layer=0;

// Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        randomOffset = Random.Range(0, 1f);
        anim.Play(AnimationName, layer, randomOffset);
    }
}