using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for all the projectiles used by weapons
/// </summary>
public class GenericProjectile : MonoBehaviour
{
    [HideInInspector]
    public Transform trans;
    public string owner;

    public float lifetime;
    public Rigidbody body;
    public float damage;

    public void Init()
    {
        trans = transform;
        body = GetComponent<Rigidbody>();
    }
}
