using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : GenericGun
{
    public float AmountOfBullets;
    public float Angle;

    public override    void Update()
    {
        timeSinceLastShot -= Time.deltaTime;

        if (timeSinceLastShot < 0 || canShoot)
        {
            timeSinceLastShot = loadingTime;
            canShoot = true;
        }
    }

    public override void Shoot()
    {
        float anglemultiplier = (Angle * 2) / (AmountOfBullets - 1);
        float currentangle = 0;

        for (int i = 0; i < AmountOfBullets; i++)
        {
            currentangle = -Angle + (anglemultiplier * i);
            var bullet = Instantiate(projectile).GetComponent<GenericProjectile>();
            bullet.Init();

            bullet.trans.position = mussle.position + new Vector3(Mathf.Cos(currentangle), 0f, Mathf.Sin(currentangle));
            bullet.trans.eulerAngles = bullet.trans.position - mussle.transform.position;
            bullet.body.velocity = mussle.forward * bulletVelocity;
            bullet.gameObject.tag = transform.parent.tag;
            bullet.owner = transform.parent.name;

            canShoot = false;
            timeSinceLastShot = loadingTime;
        }

        //var body = Instantiate(projectile).GetComponent<GenericProjectile>();
        //body.Init();

        //body.transform.position = mussle.position;
        //body.transform.rotation = transform.rotation;
        //body.body.velocity = mussle.forward * bulletVelocity;
        //body.gameObject.tag = transform.parent.tag;
        //body.owner = transform.parent.name;

        //canShoot = false;
        //timeSinceLastShot = loadingTime;
    }
}
