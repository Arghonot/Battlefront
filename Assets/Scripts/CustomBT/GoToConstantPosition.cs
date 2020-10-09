using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class GoToConstantPosition : Leaf<int>
    {
        public string RandomPositionName;
        public Vector3 positionToReach;

        public override object Run()
        {
            ((DefaultGraph)graph).gd.Set<Vector3>(
                RandomPositionName,
                positionToReach);

            return 1;
        }
    }
}