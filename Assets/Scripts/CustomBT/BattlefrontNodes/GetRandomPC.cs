using Graph;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace BT.CustomLeaves
{
    public class GetRandomPC : AILeaf
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
                    PCManager.Instance.GetRandomNeutralEnemyPC(
                        Gd.Get<Transform>("self").position,
                        Gd.Get<Team>("SelfTeam"))));

            return 1;
        }

        protected Vector3 GetRandomTargetCloseToPc(PCBehavior pc)
        {
            if (pc == null)
            {
                Debug.Log(Gd.Get<Team>("SelfTeam"));
            }

            Vector2 randV2 = UnityEngine.Random.insideUnitCircle * pc.PCRange;

            // just another security check
            if (Gd.Get<NavMeshAgent>("agent").isOnNavMesh)
            {
                Gd.Set<PCBehavior>("PcTarget", pc);

                if (pc == null)
                {
                    return
                        Gd.Get<Transform>("self").position +
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
