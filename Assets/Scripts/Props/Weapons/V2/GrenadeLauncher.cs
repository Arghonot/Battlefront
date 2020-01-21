using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Gun
{
    public override float AimValue(Vector3 pos)
    {
        return 0f;
    }

    public override bool Shoot()
    {
        throw new System.NotImplementedException();
    }
}
