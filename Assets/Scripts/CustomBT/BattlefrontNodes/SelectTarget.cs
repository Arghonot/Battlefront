using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class SelectTarget : AILeaf
    {
        public string _name;

        public LayerMask mask;

        public override object Run()
        {
            return EvaluateTargets(Spawner.Instance.getEnemiesInRange(
                Gd.Get<Team>("SelfTeam"),
                Gd.Get<Transform>("self").position,
                Gd.Get<float>("VisionDistance"))) == true ?
                1 :
                0;
        }

        bool EvaluateTargets(List<Transform> enemies)
        {

            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log((enemies == null || enemies.Count == 0) ?
                    "SelectTarget -> null or 0 enemies found" :
                    ("SelectTarget -> found : " + enemies.Count + " enemies"));
            }

            EvaluateIsInConeOfSight(ref enemies);
            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log(enemies.Count == 0 ?
                    "EvalueateInConeOfSight -> no enemies were in cone of sight" :
                    ("EvalueateInConeOfSight -> still : " + enemies.Count + " enemies remains"));
            }

            EvaluateSeables(ref enemies);
            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log((enemies == null || enemies.Count == 0) ?
                    "EvaluateSeables -> null or 0 enemies were seable" :
                    ("EvaluateSeables -> still : " + enemies.Count + " enemies were seeing."));
            }
            if (enemies.Count > 0)
            {
                Gd.Set<Transform>(
                    "Target",
                    GetClosest(enemies));
                if (Gd.Get<Transform>("self").name == _name)
                {
                    Debug.Log("EvaluateSeables -> Choose : " + Gd.Get<Transform>("Target").name);
                }

                return true;
            }

            return false;
        }

        Transform GetClosest(List<Transform> enemies)
        {
            Vector3 pos = Gd.Get<Transform>("self").position;
            int index = 0;
            float dist = float.MaxValue;
            float tmpDist = 0f;

            for (int i = 0; i < enemies.Count; i++)
            {
                tmpDist = Vector3.Distance(pos, enemies[i].position);
                if (tmpDist < dist)

                {
                    dist = tmpDist;
                    index = i;
                }
            }

            return enemies[index];
        }

        void EvaluateSeables(ref List<Transform> enemies)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (!CanBeSeen(enemies[i]))
                {
                    enemies.Remove(enemies[i]);
                }
            }
        }

        void EvaluateIsInConeOfSight(ref List<Transform> enemies)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (!IsInConeOfSight(enemies[i]))
                {
                    enemies.Remove(enemies[i]);
                }
            }
        }

        bool IsInConeOfSight(Transform enemy)
        {
            Vector3 directiontotarget = enemy.position - Gd.Get<Transform>("self").position;

            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                Gd.Get<Transform>("self").forward);

            if (seingvalue - Gd.Get<float>("VisionAngle") > 0)
            {
                return true;
            }

            return false;
        }

        bool CanBeSeen(Transform enemy)
        {
            Ray ray = new Ray(
                Gd.Get<Transform>("self").position,
                enemy.position - Gd.Get<Transform>("self").position);
            RaycastHit hit = new RaycastHit();

            if (Gd.Get<Transform>("self").name == _name)
            {
                Debug.Log("test");
            }

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Red" || hit.collider.tag == "Blue")
                {
                    return true;
                }
            }

            return false;
        }
    }
}