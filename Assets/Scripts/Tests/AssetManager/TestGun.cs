using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    public float ShootTime;
    float _shootTime;
    public float bulletVelocity;

    // Update is called once per frame
    void Update()
    {
        _shootTime += Time.deltaTime;

        if (_shootTime > ShootTime)
        {
            _shootTime = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        var body = AssetManager.Instance.Get<GenericBullet>("GenericBullet");
        body.Init();

        body.transform.position = transform.position;
        body.transform.rotation = transform.rotation;
        body.body.velocity = transform.forward * bulletVelocity;
    }
}
