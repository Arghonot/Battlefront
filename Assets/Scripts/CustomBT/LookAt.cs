using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class LookAt : Leaf<int>
    {
        public string Target;
        public string _name;

        // TODO remove ?
        Graph.GenericDicionnary AIcontext
        {
            get
            {
                return ((DefaultGraph)graph).gd;
            }
        }

        public override object Run()
        {
            if (AIcontext.Get<Transform>("self").name == _name)
            {
                Debug.Log(AIcontext.Get<Transform>(Target).name);
            }

            Transform target = AIcontext.Get<Transform>(Target);
            Transform trans = AIcontext.Get<Transform>("self");

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