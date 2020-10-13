using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunV1 : GenericGun
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
            currentangle = (-Angle + (anglemultiplier * i)) * (Mathf.PI /180f);
            var bullet = AssetManager.Instance.Get<GenericProjectile>("GenericProjectile");
            //Instantiate(projectile).GetComponent<GenericProjectile>();
            bullet.Init();

            bullet.trans.position = mussle.position + (Mathf.Sin(currentangle) * mussle.right) + (Mathf.Cos(currentangle) * mussle.forward);
            bullet.trans.LookAt(bullet.trans.position + bullet.trans.position - mussle.transform.position);
            //bullet.trans.eulerAngles = bullet.trans.position - mussle.transform.position;
            bullet.body.velocity = bullet.trans.forward * bulletVelocity;
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
