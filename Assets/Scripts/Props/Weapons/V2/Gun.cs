using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform mussle;
    public Transform projectile;

    public float bulletVelocity;

    public float loadingTime;
    protected float TimeSinceLastShot;
    public bool canShoot;

    public virtual bool Shoot()
    {
        return true;
    }

    public void Initialize(WeaponProfile profile)
    {
        bulletVelocity = profile.bulletVelocity;
        loadingTime = profile.loadingTime;
    }

    public void Update()
    {
        TimeSinceLastShot += Time.deltaTime;

        if (TimeSinceLastShot > loadingTime || canShoot)
        {
            TimeSinceLastShot = 0f;
            canShoot = true;
        }
    }

    public void Aim(Vector3 pos)
    {
        mussle.LookAt(pos);
    }

    public virtual float AimValue(Vector3 pos)
    {
        Vector3 directiontotarget = pos - transform.position;

        return Vector3.Dot(
            directiontotarget.normalized,
            mussle.forward);
    }
}
