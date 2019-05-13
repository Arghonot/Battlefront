using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGun : MonoBehaviour
{
    public GameObject grenade;
    public Transform target;
    public float grenadeVelocity;

    public bool shouldShootGrenade;

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(target);

        if (shouldShootGrenade)
        {
            var body = Instantiate(grenade).GetComponent<Rigidbody>();
            body.transform.position = transform.position;
            body.velocity = transform.forward * grenadeVelocity;

            shouldShootGrenade = false;
        }
    }
}
