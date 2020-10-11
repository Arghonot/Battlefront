using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class HasTarget : AILeaf
    {
        public string _name;
        public LayerMask mask;

        public override object Run()
        {
            Transform target = Gd.TryGet("Target") as Transform;

            if (Gd.Get<bool>("ShallDebug"))
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
                if (Gd.Get<Transform>("self").name == _name)
                {
                    Debug.Log("HasTarget == null");
                }

                Gd.Set<Transform>("Target", null);
                return 0;
            }
            if (!Spawner.Instance.isAlive(target))
            {
                Gd.Set<Transform>("Target", null);
                return 0;
            }
            if (!CheckDistance(target))
            {
                Gd.Set<Transform>("Target", null);
                return 0;
            }
            if (!IsInConeOfSight(target))
            {
                Gd.Set<Transform>("Target", null);
                return 0;
            }
            if (!CanBeSeen(target))
            {
                Gd.Set<Transform>("Target", null);
                return 0;
            }

            return 1;
        }

        bool IsInConeOfSight(Transform enemy)
        {
            Vector3 directiontotarget = enemy.position - Gd.Get<Transform>("self").position;

            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                Gd.Get<Transform>("self").forward);

            if (seingvalue - Gd.Get<float>("VisionAngle") > 0)
            {
                return true;
            }
            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log("IsInConeOfSight");
            }
            return false;
        }

        bool CanBeSeen(Transform enemy)
        {
            Ray ray = new Ray(
                Gd.Get<Transform>("self").position,
                enemy.position - Gd.Get<Transform>("self").position);
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
            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log("CanBeSeen");
            }
            return false;
        }

        bool CheckDistance(Transform target)
        {
            if (Vector3.Distance(
                Gd.Get<Transform>("self").position,
                target.position) > Gd.Get<float>("VisionDistance"))
            {
                return false;
            }
            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log("CheckDistance");
            }
            return true;
        }
    }
}