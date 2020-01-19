using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DebugTarget : MonoBehaviour
{
    PlayerAI ai;

    public bool distance = false;
    public bool angle = false;
    public bool raycast = false;

    public float aimvalue = 0f;

    public List<Transform> EnemiesInConeOfSightSTEP1;
    public List<Transform> EnemiesInConeOfSightSTEP2;
    public List<Transform> EnemiesInConeOfSightSTEP3;
    public List<Transform> EnemiesInConeOfSightSTEP4;

    void Start()
    {
        ai = GetComponent<PlayerAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ai.DebugArgs.Enemy == null)
        {
            return;
        }

        distance = DistanceCheck(ai.DebugArgs.Enemy.transform);
        angle = IsInConeOfSight(ai.DebugArgs.Enemy.transform);
        raycast = CanBeSeen(ai.DebugArgs.Enemy.transform);
        aimvalue = AimValue(ai.DebugArgs.Enemy.transform);

        LogEnemies();
    }

    void LogEnemies()
    {
        EnemiesInConeOfSightSTEP1 = Spawner.Instance.getEnemiesInRange(
            ai.selfTeam,
            transform.position,
            SoldierClassManager.Instance.GetRightVisionDistance(ai.OwnClass));

        EnemiesInConeOfSightSTEP2 = EnemiesInConeOfSightSTEP1.Where(x => DistanceCheck(x)).ToList();
        EnemiesInConeOfSightSTEP3 = EnemiesInConeOfSightSTEP2.Where(x => IsInConeOfSight(x)).ToList();
        EnemiesInConeOfSightSTEP4 = EnemiesInConeOfSightSTEP3.Where(x => CanBeSeen(x)).ToList();
    }

    bool DistanceCheck(Transform enemy)
    {
        if (Vector3.Distance(transform.position, enemy.position) >
            SoldierClassManager.Instance.GetRightVisionDistance(ai.OwnClass))
        {
            return false;
        }

        return true;
    }

    bool IsInConeOfSight(Transform enemy)
    {
        Vector3 directiontotarget = enemy.position - transform.position;

        float seingvalue = Vector3.Dot(
            directiontotarget.normalized,
            transform.forward);

        if (seingvalue - SoldierClassManager.Instance.GetRightVisionAngle(ai.OwnClass) > 0)
        {
            return true;
        }

        return false;
    }

    bool CanBeSeen(Transform enemy)
    {
        Ray ray = new Ray(
            transform.position,
            enemy.position - transform.position);
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

    float AimValue(Transform enemy)
    {
        return 0f;
    }
}
