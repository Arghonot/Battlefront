using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocator : MonoBehaviour
{
    public float visionAngle;
    public float VisionDistance;
    public Transform target;
    public Transform head;
    public Weapon gun;
    public float damping;
    public LayerMask mask;
    Ray debugray = new Ray();

    void Update()
    {
        if (target == null)
        {
            EvaluateTargets();
        }

        ManageTarget();
    }


    void EvaluateTargets()
    {
        Vector3[] enemies = TestTeamManager.Instance.GetEnemiesPositions();
        bool[] ShouldCheck = new bool[enemies.Length];

        for (int i = 0; i < ShouldCheck.Length; i++)
        {
            ShouldCheck[i] = true;
        }

        // Evaluate all their distance
        EvaluateDistance(enemies, ShouldCheck);
        ShowEnemiesSelectable(enemies, ShouldCheck, "EvaluateDistance");
        // is on cone of sight ?
        EvaluateConeOfSight(enemies, ShouldCheck);
        ShowEnemiesSelectable(enemies, ShouldCheck, "EvaluateConeOfSight");
        // Is hidden by obstacles ?
        EvaluateObstacles(enemies, ShouldCheck);
        ShowEnemiesSelectable(enemies, ShouldCheck, "EvaluateObstacles");

        GetMostInterestingTarget(enemies, ShouldCheck);
    }

    void ShowEnemiesSelectable(Vector3[] enemies, bool[] shouldcheck, string phase)
    {
        int amount = enemies.Length;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (!shouldcheck[i])
                amount -= 1;
        }

        print("[" + phase + "] There are " + amount + " selectable yet on [" + enemies.Length + "]");
    }

    void GetMostInterestingTarget(Vector3[] enemies, bool[] shouldcheck)
    {
        float distance = float.PositiveInfinity;
        int indextotake= -1;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (shouldcheck[i])
            {
                if (Vector3.Distance(transform.position, enemies[i]) < distance)
                {
                    print(distance + "  " + i);
                    indextotake = i;
                    distance = Vector3.Distance(transform.position, enemies[i]);
                }

            }
        }

        if (indextotake != -1)
        {
            print("finally took : " + indextotake + "   " + distance);
            target = TestTeamManager.Instance.GetPlayerTransformFromIndex(indextotake);
        }
        return;
    }

    void EvaluateObstacles(Vector3[] enemies, bool[] shouldcheck)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (shouldcheck[i])
            {
                if (!LineofSightClear(enemies[i]))
                {
                    shouldcheck[i] = false;
                }
            }
        }
    }

    void EvaluateConeOfSight(Vector3[] enemies, bool[] shouldcheck)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (shouldcheck[i])
            {
                if (!isInConeOfSight(enemies[i]))
                {
                    shouldcheck[i] = false;
                }
            }
        }
    }

    bool isInConeOfSight(Vector3 obj)
    {
        Vector3 directiontotarget = obj - head.position;

        float seingvalue = Vector3.Dot(directiontotarget.normalized, head.forward);

        if (seingvalue - visionAngle > 0)
        {
            return true;
        }

        return false;
    }

    void EvaluateDistance(Vector3[] enemies, bool[] shouldcheck)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (isEnemyTooFar(enemies[i]))
            {
                shouldcheck[i] = false;
            }
        }
    }

    bool isEnemyTooFar(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) > VisionDistance)
            return true;

        return false;
    }

    void ManageTarget()
    {
        bool enemyInSight = false;

        if (target == null)
            return;

        // We re evaluate the quality of our target
        if (!isEnemyTooFar(target.position))
        {
            if (isInConeOfSight(target.position))
            {
                enemyInSight = LineofSightClear(target.position);
            }
        }

        // If our target is not good anymore
        if (!enemyInSight)
        {
            print("null");
            target = null;
            return;
        }

        // else we adjust our rotation and our gun's
        CustomPlayerLookAt();
        gun.CustomLookAt(target.position);

        // if we can shoot
        if (gun.canShoot)
        {
            gun.Shoot();
        }
    }

    void CustomPlayerLookAt()
    {
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    bool LineofSightClear(Vector3 targetposition)
    {
        Ray ray = new Ray(head.position, targetposition - head.position);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, mask))
        {
            if (hit.collider.tag == "Red")
            {
                return true;
            }
        }

        debugray = ray;

        return false;
    }

    private void OnDrawGizmos()
    {

        debugray.direction *= 100f;
         Gizmos.DrawRay(debugray);
    }
}
