using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the main class all the weapons shall inherit from
public class GenericGun : MonoBehaviour
{
    public GenericProjectile projectile;
    public Transform mussle;
    public bool canShoot;
    public float loadingTime;
    float timeSinceLastShot;
    public float bulletVelocity;
    public float rotationSpeed;

    void Update()
    {
        timeSinceLastShot -= Time.deltaTime;

        if (timeSinceLastShot < 0 || canShoot)
        {
            timeSinceLastShot = loadingTime;
            canShoot = true;
        }
    }

    public void CustomLookAt(Vector3 target)
    {
        var targetRotation = Quaternion.LookRotation(target - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        var body = Instantiate(projectile).GetComponent<GenericProjectile>();
        body.Init();

        body.transform.position = mussle.position;
        body.transform.rotation = transform.rotation;
        body.body.velocity = mussle.forward * bulletVelocity;
        body.gameObject.tag = transform.parent.tag;
        body.owner = transform.parent.name;

        canShoot = false;
        timeSinceLastShot = loadingTime;
    }
}
