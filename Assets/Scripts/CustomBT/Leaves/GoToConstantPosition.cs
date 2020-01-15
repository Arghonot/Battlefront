using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class GoToConstantPosition : BTNode
    {
        public string RandomPositionName;
        public Vector3 positionToReach;

        public override BTState Run()
        {
            AIcontext.Set<Vector3>(
                RandomPositionName,
                positionToReach);

            return BTState.Success;
        }
    }
}