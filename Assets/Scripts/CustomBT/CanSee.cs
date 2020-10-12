using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace BT.CustomLeaves
{
    public class CanSee : AILeaf
    {
        public override object Run()
        {
            if (Gd.Get<bool>("DebugCanSee"))
            {
                CanBeSeen(Gd.Get<Transform>("Target"));
            }

            return CanBeSeen(Gd.Get<Transform>("Target")) ?
                1 :
                0;
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

            return false;
        }
    }
}