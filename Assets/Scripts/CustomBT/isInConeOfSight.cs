using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class isInConeOfSight : AILeaf
    {
        public string ConeOfSight;
        public string TargetName;

        public override object Run()
        {
            // If something wasn't set
            if (Gd.Get<Transform>(TargetName) == null ||
                Gd.Get<float>(ConeOfSight) == 0f)
            {
                return 0;
            }

            return IsInConeOfSight(
                    Gd.Get<Transform>(TargetName),
                    Gd.Get<float>(ConeOfSight)) ?
                1 :
                0;
        }

        bool IsInConeOfSight(Transform target, float visionAngle)
        {
            Vector3 directiontotarget = 
                target.position -
                Gd.Get<Transform>("self").position;

            // we use the gun instead of player's transform in case of slope
            // in this case the player might look at target perfectly but the elevation
            // will make him consider it is not seing it properly
            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                Gd.Get<Gun>("Gun").sight.forward);

            if (Gd.Get<bool>("DebugMussle"))
            {
                Debug.Log(seingvalue);
            }

            if (seingvalue - visionAngle > 0)
            {
                return true;
            }

            return false;
        }
    }
}