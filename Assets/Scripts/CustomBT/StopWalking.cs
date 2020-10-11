using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT.StandardLeaves
{
    public class StopWalking : AILeaf
    {
        public override object Run()
        {
            var agent = Gd.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                Debug.Log("Couldn't find any agent");
                return 0;
            }

            agent.isStopped = true;
            
            return 1;
        }
    }
}