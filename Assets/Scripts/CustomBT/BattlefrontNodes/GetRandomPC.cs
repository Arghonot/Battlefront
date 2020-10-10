using Graph;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace BT.CustomLeaves
{
    public class GetRandomPC : Leaf<int>
    {
        public string AreaToUse;
        public string RandomPositionName;

        public bool UseNavmesh;
        public float WalkRadius;

        public override object Run()
        {
            ((DefaultGraph)graph).gd.Set<Vector3>(
                RandomPositionName,
                GetRandomTargetCloseToPc(
                    PCManager.Instance.GetFirstNeutralEnemyPC(
                        ((DefaultGraph)graph).gd.Get<Transform>("self").position,
                        ((DefaultGraph)graph).gd.Get<Team>("SelfTeam"),
                        ((DefaultGraph)graph).gd.Get<bool>("ShallDebug"))));

            return 1;
        }

        protected Vector3 GetRandomTargetCloseToPc(PCBehavior pc)
        {
            if (pc == null)
            {
                Debug.Log(((DefaultGraph)graph).gd.Get<Team>("SelfTeam"));
            }

            Vector2 randV2 = UnityEngine.Random.insideUnitCircle * pc.PCRange;

            // just another security check
            if (((DefaultGraph)graph).gd.Get<NavMeshAgent>("agent").isOnNavMesh)
            {
                ((DefaultGraph)graph).gd.Set<PCBehavior>("PcTarget", pc);

                if (pc == null)
                {
                    return
                        ((DefaultGraph)graph).gd.Get<Transform>("self").position +
                        new Vector3(randV2.x, 1f, randV2.y);
                }
                else
                {
                    // We set target  to some place around the pc (but still inside)
                    return pc.transform.position + new Vector3(randV2.x, 1f, randV2.y);
                }
            }

            return pc.trans.position;
        }
    }
}
