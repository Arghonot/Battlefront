using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for all the projectiles used by weapons
/// </summary>
public class GenericProjectile : PoolableObject
{
    [HideInInspector]
    public Transform trans;
    public string owner;

    protected float CurrentLifetime;
    public float lifetime;
    public Rigidbody body;
    public float damage;

    bool isInit = false;

    private void OnEnable()
    {
        //Debug.Log(CurrentLifetime + "   " + lifetime);
        CurrentLifetime = lifetime;
    }

    public void Init()
    {
        if (!isInit)
        {
            trans = transform;
            body = GetComponent<Rigidbody>();
            isInit = true;
        }
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
    }
}
