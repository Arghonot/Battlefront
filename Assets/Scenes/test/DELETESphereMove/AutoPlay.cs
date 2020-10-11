using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim.Play("SphereMoving");
    }
}
