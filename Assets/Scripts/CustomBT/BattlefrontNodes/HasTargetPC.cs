using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace BT.CustomLeaves
{
    public class HasTargetPC : AILeaf
    {
        public string RandomPositionName;

        public override object Run()
        {
            if (Gd.Get<Vector3>(
                    RandomPositionName) != Vector3.zero)
            {
                return 1;
            }

            return 0;
        }
    }
}