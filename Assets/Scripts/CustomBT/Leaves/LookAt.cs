using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class LookAt : BTNode
    {
        public string Target;
        public string _name;

        public override BTState Run()
        {
            if (AIcontext.Get<Transform>("self").name == _name)
            {
                Debug.Log(AIcontext.Get<Transform>(Target).name);
            }

            Transform target = AIcontext.Get<Transform>(Target);
            Transform trans = AIcontext.Get<Transform>("self");

            if (target == null || trans == null)
            {
                return BTState.Failure;
            }

            var targetPoint = target.position;
            var targetRotation = Quaternion.LookRotation(targetPoint - trans.position, Vector3.up);
            trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, Time.deltaTime);

            return BTState.Success;
        }
    }
}