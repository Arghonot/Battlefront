using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class LookAt : AILeaf
    {
        public string Target;
        public string _name;

        public override object Run()
        {
            Gd.Set<bool>("ikActive", true);

            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log(Gd.Get<Transform>(Target).name);
            }

            Transform target = Gd.Get<Transform>(Target);
            Transform trans = Gd.Get<Transform>("self");

            if (target == null || trans == null)
            {
                return 0;
            }

            var targetPoint = target.position;
            var targetRotation =
                Quaternion.LookRotation(targetPoint - trans.position, Vector3.up);
            trans.rotation =
                Quaternion.Slerp(trans.rotation, targetRotation, Time.deltaTime);

            return 1;
        }
    }
}