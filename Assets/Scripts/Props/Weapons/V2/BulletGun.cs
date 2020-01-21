using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : Gun
{
    public override bool Shoot()
    {
        if (!canShoot)
        {
            return false;
        }

        var body = Instantiate(projectile).GetComponent<GenericProjectile>();
        body.Init();

        body.transform.position = mussle.position;
        body.transform.rotation = mussle.rotation;
        body.body.velocity = mussle.forward * bulletVelocity;
        body.owner = transform.parent.name;

        canShoot = false;
        TimeSinceLastShot = 0f;

        return true;
    }
}
