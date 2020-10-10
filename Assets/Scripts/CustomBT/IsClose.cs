using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.StandardLeaves
{
    public class IsClose : AILeaf
    {
        public string DistanceSource;
        public string ObjectA;
        public string ObjectB;

        public override object Run()
        {
            var objA = Gd.Get<Transform>(ObjectA);
            var objB = Gd.Get<Transform>(ObjectB);
            float distance;

            if (objA == null || objB == null)
            {
                return 0;
            }

            distance = Vector3.Distance(objA.position, objB.position);

            if (Gd.Get<bool>("ShallDebug"))
            {
                Debug.Log("IsClose [distance]" + distance);
            }

            if (distance > Gd.Get<float>(DistanceSource))
            {
                return 0;
            }

            return 1;
        }
    }
}