using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the main class all the weapons shall inherit from
public class GenericGun : MonoBehaviour
{
    public GameObject projectile;
    public Transform mussle;
    public Transform parent;
    public bool canShoot;
    public float loadingTime;
    protected float timeSinceLastShot;
    public float bulletVelocity;
    public float rotationSpeed;

    public virtual void Update()
    {
        timeSinceLastShot -= Time.deltaTime;

        if (timeSinceLastShot < 0 || canShoot)
        {
            timeSinceLastShot = loadingTime;
            canShoot = true;
        }
    }

    public virtual  void CustomLookAt(Vector3 target)
    {
        var targetRotation = Quaternion.LookRotation(target - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public  virtual void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        var body = Instantiate(projectile).GetComponent<GenericProjectile>();
        body.Init();

        body.transform.position = mussle.position;
        body.transform.rotation = mussle.rotation;
        body.body.velocity = mussle.forward * bulletVelocity;
        body.gameObject.tag = parent.tag;
        body.owner = transform.parent.name;

        canShoot = false;
        timeSinceLastShot = loadingTime;
    }
}
