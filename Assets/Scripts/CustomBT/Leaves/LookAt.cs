using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class LookAt : BTNode
    {
        public string Target;

        public override BTState Run()
        {
            Transform target = AIcontext.Get<Transform>(Target);
            Transform trans = AIcontext.Get<Transform>("self");

            if (target == null || trans == null)
            {
                return BTState.Failure;
            }

            var targetPoint = target.position;
            var targetRotation = Quaternion.LookRotation(targetPoint - trans.position, Vector3.up);
            trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, Time.deltaTime * 2.0f);

            return BTState.Success;
        }
    }
}