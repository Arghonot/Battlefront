using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
}
