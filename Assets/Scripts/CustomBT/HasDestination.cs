using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT.StandardLeaves
{
    public class HasDestination : AILeaf
    {
        //[Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        //public NavMeshAgent agent;

        //[Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public string DestinationPath;

        public override object Run()
        {
            var agent = Gd.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                // We have to have a NavMeshAgent on this agent
                return 0;
                //throw new System.Exception();
            }

            return HasReachedDestination();
        }

        int HasReachedDestination()
        {
            var navagent = Gd.Get<NavMeshAgent>("agent");

            if (!navagent.hasPath)
            {
                return 0;
            }

            float dist = Vector3.Distance(navagent.gameObject.transform.position,
                Gd.Get<Vector3>(DestinationPath));

            if (navagent.pathPending)
            {
                return 1;
            }

            if (navagent.pathStatus == NavMeshPathStatus.PathComplete &&
                navagent.remainingDistance == 0)
            {
                return 0;
            }

            if (navagent.pathStatus == NavMeshPathStatus.PathPartial ||
                dist < 3f)
            {
                return 0;
            }

            return 1;
        }
    }
}