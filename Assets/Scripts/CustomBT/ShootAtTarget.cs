using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class ShootAtTarget : BTNode
    {
        public override BTState Run()
        {
            if (AIcontext.Get<Gun>("Gun").canShoot)
            {
                AIcontext.Get<Gun>("Gun").Shoot();
            }

            return BTState.Success;
        }
    }
}