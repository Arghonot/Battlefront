using System;
using UnityEngine;
using UnityEngine.AI;

namespace BT.CustomLeaves
{
    public class GetRandomPC : Leaves.GetRandomPosition
    {
        public override BTState Run()
        {
            AIcontext.Set<Vector3>(
                RandomPositionName,
                GetRandomTargetCloseToPc(
                    PCManager.Instance.GetFirstNeutralEnemyPC(
                        AIcontext.Get<Transform>("self").position,
                        AIcontext.Get<Team>("SelfTeam"),
                        AIcontext.Get<bool>("ShallDebug"))));

            return BTState.Success;
        }

        protected Vector3 GetRandomTargetCloseToPc(PCBehavior pc)
        {
            if (pc == null)
            {
                Debug.Log(AIcontext.Get<Team>("SelfTeam"));
            }

            Vector2 randV2 = UnityEngine.Random.insideUnitCircle * pc.PCRange;

            // just another security check
            if (AIcontext.Get<NavMeshAgent>("agent").isOnNavMesh)
            {
                AIcontext.Set<PCBehavior>("PcTarget", pc);

                if (pc == null)
                {
                    return AIcontext.Get<Transform>("self").position + new Vector3(randV2.x, 1f, randV2.y);
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
