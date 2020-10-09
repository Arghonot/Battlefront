using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class isInConeOfSight : Leaf<int>
    {
        public string ConeOfSight;
        public string TargetName;

        public override object Run()
        {
            // If something wasn't set
            if (((DefaultGraph)graph).gd.Get<Transform>(TargetName) == null ||
                ((DefaultGraph)graph).gd.Get<float>(ConeOfSight) == 0f)
            {
                return 0;
            }

            return IsInConeOfSight(
                    ((DefaultGraph)graph).gd.Get<Transform>(TargetName),
                    ((DefaultGraph)graph).gd.Get<float>(ConeOfSight)) ?
                1 :
                0;
        }

        bool IsInConeOfSight(Transform target, float visionAngle)
        {
            Vector3 directiontotarget = 
                target.position -
                ((DefaultGraph)graph).gd.Get<Transform>("self").position;

            // we use the gun instead of player's transform in case of slope
            // in this case the player might look at target perfectly but the elevation
            // will make him consider it is not seing it properly
            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                ((DefaultGraph)graph).gd.Get<Gun>("Gun").mussle.forward);

            if (((DefaultGraph)graph).gd.Get<bool>("DebugMussle"))
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