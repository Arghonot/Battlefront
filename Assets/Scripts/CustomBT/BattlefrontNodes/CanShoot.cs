using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace BT.CustomLeaves
{
    public class CanShoot : AILeaf
    {
        public override object Run()
        {
            bool canGunShoot = Gd.Get<GunBehavior>("Gun").CanShoot();

            if (canGunShoot && isInConeOfSight(
                Gd.Get<Transform>("self"),
                Gd.Get<Transform>("target")))
            {
                return 1;
            }

            return 0;
        }

        bool isInConeOfSight(Transform self, Transform target)
        {
            Vector3 directiontotarget = target.position - self.position;

            float seingvalue =
                Vector3.Dot(directiontotarget.normalized, self.forward) *
                Mathf.Rad2Deg;

            if (seingvalue - 30f > 0)
            {
                return true;
            }

            return false;
        }
    }
}