using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class HasTarget : BTNode
    {
        public string _name;
        public LayerMask mask;

        public override BTState Run()
        {
            Transform target = AIcontext.Get<Transform>("Target");

            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log(target == null ? "HasTarget -> no" : ("HasTarget -> yes " + target.name));
            }

            //        if (target == null ||
            //!Spawner.Instance.isAlive(target) ||
            //!CheckDistance(target) ||
            //!IsInConeOfSight(target) ||
            //!CanBeSeen(target))
            //        {
            //            AIcontext.Set<Transform>("Target", null);
            //            return BTState.Failure;
            //        }
            if (target == null)
            {
                if (AIcontext.Get<Transform>("self").name == _name)
                {
                    Debug.Log("HasTarget == null");
                }

                AIcontext.Set<Transform>("Target", null);
                return BTState.Failure;
            }
            if (!Spawner.Instance.isAlive(target))
            {
                AIcontext.Set<Transform>("Target", null);
                return BTState.Failure;
            }
            if (!CheckDistance(target))
            {
                AIcontext.Set<Transform>("Target", null);
                return BTState.Failure;
            }
            if (!IsInConeOfSight(target))
            {
                AIcontext.Set<Transform>("Target", null);
                return BTState.Failure;
            }
            if (!CanBeSeen(target))
            {
                AIcontext.Set<Transform>("Target", null);
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
            if (AIcontext.Get<Transform>("self").name == _name)
            {
                Debug.Log("IsInConeOfSight");
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
                    if (hit.collider.name == enemy.name ||
                        hit.collider.tag == enemy.tag)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (AIcontext.Get<Transform>("self").name == _name)
            {
                Debug.Log("CanBeSeen");
            }
            return false;
        }

        bool CheckDistance(Transform target)
        {
            if (Vector3.Distance(
                AIcontext.Get<Transform>("self").position,
                target.position) > AIcontext.Get<float>("VisionDistance"))
            {
                return false;
            }
            if (AIcontext.Get<Transform>("self").name == _name)
            {
                Debug.Log("CheckDistance");
            }
            return true;
        }
    }
}