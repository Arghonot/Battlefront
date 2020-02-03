using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.CustomLeaves
{
    public class CanSee : BTNode
    {
        public override BTState Run()
        {
            if (AIcontext.Get<bool>("DebugCanSee"))
            {
                CanBeSeen(AIcontext.Get<Transform>("Target"));
            }

            return CanBeSeen(AIcontext.Get<Transform>("Target")) ?
                BTState.Success :
                BTState.Failure;
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

            return false;
        }
    }
}