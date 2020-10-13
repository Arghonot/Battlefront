using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.CustomTests
{
    public class DisplayTargetSeeing : AILeaf
    {
        public override object Run()
        {
            if (Gd.Get<Team>("SelfTeam") == Team.Red)
            {
                return 1;
            }

            Vector3 directiontotarget =
                    RuntimeTestManager.Instance.Target.position -
                Gd.Get<Transform>("self").position;

            // we use the gun instead of player's transform in case of slope
            // in this case the player might look at target perfectly but the elevation
            // will make him consider it is not seing it properly
            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                Gd.Get<Gun>("Gun").sight.forward);

            Debug.Log(seingvalue);

            return 1;
        }
    }
}