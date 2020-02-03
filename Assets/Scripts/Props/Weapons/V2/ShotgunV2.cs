using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunV2 : Gun
{
    public float AmountOfBullets;
    public float Angle;

    public override bool Shoot()
    {
        if (!canShoot)
        {
            return false;
        }

        float anglemultiplier = (Angle * 2) / (AmountOfBullets - 1);
        float currentangle = 0;

        for (int i = 0; i < AmountOfBullets; i++)
        {
            currentangle = (-Angle + (anglemultiplier * i)) * (Mathf.PI / 180f);
            var bullet = Instantiate(projectile).GetComponent<GenericProjectile>();
            bullet.Init();

            bullet.trans.position = mussle.position + (Mathf.Sin(currentangle) * mussle.right) + (Mathf.Cos(currentangle) * mussle.forward);
            bullet.trans.LookAt(bullet.trans.position + bullet.trans.position - mussle.transform.position);
            bullet.body.velocity = bullet.trans.forward * bulletVelocity;
            bullet.gameObject.tag = transform.parent.tag;
            bullet.owner = transform.parent.name;

            canShoot = false;
        }

        TimeSinceLastShot = 0f;

        return true;
    }
}
