using UnityEngine;
using Graph;
using UnityEngine.AI;

namespace BT.StandardLeaves
{
    public class GetRandomPosition : AILeaf
    {
        public string AreaToUse;
        public string RandomPositionName;

        public bool UseNavmesh;
        public float WalkRadius;

        public override object Run()
        {
            if (UseNavmesh)
            {
                Gd.Set<Vector3>(
                    RandomPositionName,
                    GetRandomPositionOnNavmesh());

                return 1;
            }

            return GetRandomFromBounds();
        }

        protected object GetRandomFromBounds()
        {
            var area = Gd.Get<Collider>(AreaToUse);

            if (area == null)
            {
                return 0;
                // We have to have a NavMeshAgent on this agent
                //throw new System.Exception();
            }

            Gd.Set<Vector3>(
                RandomPositionName,
                GetRandom(area.bounds));

            return 1;
        }

        protected Vector3 GetRandomPositionOnNavmesh()
        {
            Transform self = Gd.Get<Transform>("self");

            Vector3 randomDirection = self.position +
               Random.insideUnitSphere *
               WalkRadius;

            randomDirection += self.position;
            NavMeshHit hit;

            if (!NavMesh.SamplePosition(randomDirection, out hit, WalkRadius, 1))
            {
                return self.position;
            }

            Debug.Log(hit.position);

            return hit.position;
        }

        protected Vector3 GetRandom(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));
        }
    }
}