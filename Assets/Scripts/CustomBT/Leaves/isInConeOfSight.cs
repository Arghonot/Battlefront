using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class isInConeOfSight : BTNode
    {
        public string ConeOfSight;
        public string TargetName;

        public override BTState Run()
        {
            // If something wasn't set
            if (AIcontext.Get<Transform>(TargetName) == null ||
                AIcontext.Get<float>(ConeOfSight) == 0f)
            {
                return BTState.Failure;
            }

            return IsInConeOfSight(
                    AIcontext.Get<Transform>(TargetName),
                    AIcontext.Get<float>(ConeOfSight)) ?
                BTState.Success :
                BTState.Failure;
        }

        bool IsInConeOfSight(Transform target, float visionAngle)
        {
            Vector3 directiontotarget = 
                target.position -
                AIcontext.Get<Transform>("self").position;

            // we use the gun instead of player's transform in case of slope
            // in this case the player might look at target perfectly but the elevation
            // will make him consider it is not seing it properly
            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                AIcontext.Get<Gun>("Gun").mussle.forward);

            if (seingvalue - visionAngle > 0)
            {
                return true;
            }

            return false;
        }
    }
}