using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class HasTarget : BTNode
    {
        public override BTState Run()
        {
            Transform target = AIcontext.Get<Transform>("Target");

            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log(target == null ? "HasTarget -> no" : ("HasTarget -> yes " + target.name));
            }

            if (target == null ||
                !CheckDistance(target) ||
                !IsInConeOfSight(target) ||
                !CanBeSeen(target))
            {
                return BTState.Failure;
            }

            return BTState.Success;
        }

        bool IsInConeOfSight(Transform enemy)
        {
            Vector3 directiontotarget = enemy.position - AIcontext.Get<Transform>("self").position;

            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                AIcontext.Get<Transform>("self").forward);

            if (seingvalue - AIcontext.Get<float>("VisionAngle") > 0)
            {
                return true;
            }

            return false;
        }

        bool CanBeSeen(Transform enemy)
        {
            Ray ray = new Ray(
                AIcontext.Get<Transform>("self").position,
                enemy.position - AIcontext.Get<Transform>("self").position);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Red" || hit.collider.tag == "Blue")
                {
                    return true;
                }
            }

            return false;
        }

        bool CheckDistance(Transform target)
        {
            if (Vector3.Distance(
                AIcontext.Get<Transform>("self").position,
                target.position) > AIcontext.Get<float>(""))
            {
                return false;
            }

            return true;
        }
    }
}