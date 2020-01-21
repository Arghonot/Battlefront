using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public Transform mussle;
    public Transform projectile;

    public float bulletVelocity;

    public float loadingTime;
    protected float TimeSinceLastShot;
    public bool canShoot;

    public abstract bool Shoot();

    public virtual void Update()
    {
        TimeSinceLastShot += Time.deltaTime;

        if (TimeSinceLastShot > loadingTime || canShoot)
        {
            TimeSinceLastShot = 0f;
            canShoot = true;
        }
    }

    public virtual float AimValue(Vector3 pos)
    {
        Vector3 directiontotarget = pos - transform.position;

        return Vector3.Dot(
            directiontotarget.normalized,
            mussle.forward);
    }
}
