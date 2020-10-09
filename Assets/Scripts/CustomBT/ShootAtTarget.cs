using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class ShootAtTarget : Leaf<int>
    {
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
            if (AIcontext.Get<Gun>("Gun").canShoot)
            {
                AIcontext.Get<Gun>("Gun").Shoot();
            }

            return 1;
        }
    }
}