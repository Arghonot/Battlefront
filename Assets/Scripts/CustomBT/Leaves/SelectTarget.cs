using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BT.CustomLeaves
{
    public class SelectTarget : BTNode
    {
        public LayerMask mask;

        public override BTState Run()
        {
            return EvaluateTargets(Spawner.Instance.getEnemiesInRange(
                AIcontext.Get<Team>("SelfTeam"),
                AIcontext.Get<Transform>("self").position,
                AIcontext.Get<float>("VisionDistance"))) == true ?
                BTState.Success :
                BTState.Failure;
        }

        bool EvaluateTargets(List<Transform> enemies)
        {

            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log((enemies == null || enemies.Count == 0) ?
                    "SelectTarget -> null or 0 enemies found" :
                    ("SelectTarget -> found : " + enemies.Count + " enemies"));
            }

            EvalueateInConeOfSight(ref enemies);
            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log(enemies.Count == 0 ?
                    "EvalueateInConeOfSight -> no enemies were in cone of sight" :
                    ("EvalueateInConeOfSight -> still : " + enemies.Count + " enemies remains"));
            }

            EvaluateSeables(ref enemies);
            if (AIcontext.Get<bool>("ShallDebug"))
            {
                Debug.Log((enemies == null || enemies.Count == 0) ?
                    "EvaluateSeables -> null or 0 enemies were seable" :
                    ("EvaluateSeables -> still : " + enemies.Count + " enemies were seeing."));
            }
            if (enemies.Count > 0)
            {
                AIcontext.Set<Transform>(
                    "Target",
                    enemies[Random.Range(0, enemies.Count)]);
                if (AIcontext.Get<bool>("ShallDebug"))
                {
                    Debug.Log("EvaluateSeables -> Choose : " + AIcontext.Get<Transform>("Target").name);
                }

                return true;
            }

            return false;
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

        void EvalueateInConeOfSight(ref List<Transform> enemies)
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
            Vector3 directiontotarget = enemy.position - AIcontext.Get<Transform>("self").position;

            float seingvalue = Vector3.Dot(
                directiontotarget.normalized,
                AIcontext.Get<Transform>("self").forward);

            if (seingvalue - AIcontext.Get<float>("VisionAngle") > 0)
            {
                return true;
            }

            return false;
        }

        bool CanBeSeen(Transform enemy)
        {
            Ray ray = new Ray(
                AIcontext.Get<Transform>("self").position,
                enemy.position - AIcontext.Get<Transform>("self").position);
            RaycastHit hit = new RaycastHit();

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