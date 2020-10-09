using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace BT.CustomLeaves
{
    public class CanSee : Branch<int>
    {
        public override object Run()
        {
            if (((DefaultGraph)graph).gd.Get<bool>("DebugCanSee"))
            {
                CanBeSeen(((DefaultGraph)graph).gd.Get<Transform>("Target"));
            }

            return CanBeSeen(((DefaultGraph)graph).gd.Get<Transform>("Target")) ?
                1 :
                0;
        }

        bool CanBeSeen(Transform enemy)
        {
            Ray ray = new Ray(
                ((DefaultGraph)graph).gd.Get<Transform>("self").position,
                enemy.position - ((DefaultGraph)graph).gd.Get<Transform>("self").position);
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