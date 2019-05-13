using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject Bullet;
    public Transform CanonMusle;
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
        var body = Instantiate(Bullet).GetComponent<Bullet>();
        body.Init();

        body.transform.position = CanonMusle.position;
        body.transform.rotation = transform.rotation;
        body.body.velocity = CanonMusle.forward * bulletVelocity;
        body.gameObject.tag = transform.parent.tag;
        body.OwnerName = transform.parent.name;

        canShoot = false;
        timeSinceLastShot = loadingTime;
    }
}
