using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT.StandardLeaves
{
    public class Follow : AILeaf
    {
        public float RefreshRate;
        public string Target;

        private float _currentRefreshRate;

        public override object Run()
        {
            _currentRefreshRate += Time.deltaTime;

            if (_currentRefreshRate > RefreshRate)
            {
                _currentRefreshRate = 0f;

                return FollowTarget() ? 1 : 0;
            }

            return 0;
        }

        bool FollowTarget()
        {
            NavMeshAgent agent = Gd.Get<NavMeshAgent>("agent");
            Transform target = Gd.Get<Transform>(Target);

            return agent.SetDestination(target.position);
        }
    }
}