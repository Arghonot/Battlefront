using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XNode;

namespace BT.CustomLeaves
{
    public class StartWalking : BTNode
    {
        public override BTState Run()
        {
            var agent = AIcontext.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                Debug.Log("Couldn't find any agent");
                return BTState.Failure;
            }
            
            agent.isStopped = false;

            return BTState.Success;
        }
    }
}