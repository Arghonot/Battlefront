using UnityEngine;
using UnityEngine.AI;
using XNode;
using Graph;

namespace BT.StandardLeaves
{
    public class GoToPosition : AILeaf
    {
        public string PositionToReach;

        public override object Run()
        {
            var agent = Gd.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                Debug.Log("Couldn't find any agent");
            }

            agent.SetDestination(
                Gd.Get<Vector3>(PositionToReach));

            return 1;
        }
    }
}
