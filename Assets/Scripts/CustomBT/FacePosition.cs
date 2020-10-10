using UnityEngine;

namespace BT.StandardLeaves
{
    public class FacePosition : AILeaf
    {
        public string TransformToBeFaced;

        public override object Run()
        {
            Transform self = Gd.Get<Transform>("self");
            Transform target = Gd.Get<Transform>(TransformToBeFaced);
            Vector3 lookPos =
                target.position -
                self.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);

            self.rotation = Quaternion.Slerp(
                self.rotation, rotation,
                0.25f);

            if (isInConeOfSight(self, target))
            {
                return 1;
            }

            return 0;
        }

        bool isInConeOfSight(Transform self, Transform target)
        {
            Vector3 directiontotarget = target.position - self.position;

            float seingvalue =
                Vector3.Dot(directiontotarget.normalized, self.forward) *
                Mathf.Rad2Deg;

            if (seingvalue - 30f > 0)
            {
                return true;
            }

            return false;
        }
    }
}