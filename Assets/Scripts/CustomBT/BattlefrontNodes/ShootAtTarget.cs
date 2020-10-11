using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class ShootAtTarget : AILeaf
    {
        public override object Run()
        {
            if (Gd.Get<Gun>("Gun").canShoot)
            {
                Gd.Get<Gun>("Gun").Aim(Gd.Get<Transform>("Target").position);
                Gd.Get<Gun>("Gun").Shoot();
            }

            return 1;
        }
    }
}