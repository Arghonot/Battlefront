using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    PlayerAI ai;

    public float visionAngle;
    public float VisionDistance;
    public Transform target;
    public Transform head;
    //    public Weapon gun;
    public GenericGun gun;
    public float damping;
    public LayerMask mask;

    // This bool is meant to be disabled when player is under an explosion for example
    public bool shouldHandleEnemies = true;

    private void Start()
    {
        ai = GetComponent<PlayerAI>();
    }

    public void Init()
    {
        ai = gameObject.GetComponent<PlayerAI>();

        // Instantiate gun
        gun = Instantiate(AssetManager.Instance.getGunForClass(SoldierClassManager.Instance.GetRightWeaponForClass(ai.OwnClass)));
        gun.transform.SetParent(transform);
    }

    void Update()
    {
        if (!shouldHandleEnemies)
            return;

        if (target == null)
        {
            EvaluateTargets();
        }

        ManageTarget();
    }

    void EvaluateTargets()
    {
        Vector3[] enemies = Spawner.Instance.GetEnemiesPositions(ai.selfTeam);
        bool[] ShouldCheck = new bool[enemies.Length];

        for (int i = 0; i < ShouldCheck.Length; i++)
        {
            ShouldCheck[i] = true;
        }

        // Evaluate all their distance
        EvaluateDistance(enemies, ShouldCheck);
        // is on cone of sight ?
        EvaluateConeOfSight(enemies, ShouldCheck);
        // Is hidden by obstacles ?
        EvaluateObstacles(enemies, ShouldCheck);

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
    }

    void GetMostInterestingTarget(Vector3[] enemies, bool[] shouldcheck)
    {
        float distance = float.PositiveInfinity;
        int indextotake = -1;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (shouldcheck[i])
            {
                if (Vector3.Distance(transform.position, enemies[i]) < distance)
                {
                    indextotake = i;
                    distance = Vector3.Distance(transform.position, enemies[i]);
                }

            }
        }

        if (indextotake != -1)
        {
            target = Spawner.Instance.GetPlayerTransformFromIndex(indextotake, ai.selfTeam);
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
        {
            ai.agent.isStopped = false;
            return;
        }

        ai.agent.isStopped = true;
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
            if (hit.collider.tag == "Red" || hit.collider.tag == "Blue")
            {
                return true;
            }
        }

        return false;
    }
}
