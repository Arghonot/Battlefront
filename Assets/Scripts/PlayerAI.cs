using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public SoldierClass Specialities;
    TargetSelector selector;
    public Transform trans;
    public PCBehavior PCTarget;
    public Team selfTeam;
    public bool IsAlive;
    public Transform canon;

    float healthpoint = 100f;

    public NavMeshAgent agent;

    private void Update()
    {
        CheckPCRules();
    }

    /// <summary>
    /// This function check if we are trying to capture is already on our team.
    /// </summary>
    void CheckPCRules()
    {
        // If the game is over there is no need to proceed;
        if (!GameManager.Instance.IsGameRunning)
        {
            DisableSelf();

            return;
        }

        if (PCTarget.ControlledBy == selfTeam)
        {
            ChoosePCTarget();
        }

    }

    void DisableSelf()
    {
        agent.enabled = false;
        selector.enabled = false;
        this.enabled = false;
    }

    public void Init(Team team)
    {
        selfTeam = team;
        trans = transform;
        IsAlive = true;

        // setup it's own color
        GetComponent<MeshRenderer>().material.color = team == Team.Blue ? Color.blue : team == Team.Red ? Color.red : Color.yellow;
        agent = GetComponent<NavMeshAgent>();
        selector = GetComponent<TargetSelector>();
    }

    public void DispatchPlayer(PCBehavior pc)
    {
        SetupSpawnPosition(pc);
        ChoosePCTarget();
        ResetOwnStat();
    }

    void ResetOwnStat()
    {
        healthpoint = SoldierClassManager.Instance.SoldierHealthPoints;
        selector.target = null;
    }

    void SetupSpawnPosition(PCBehavior pc)
    {
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.enabled = false;

        Vector2 randV2 = Random.insideUnitCircle * pc.PCRange;
        trans.position = pc.trans.position + new Vector3(randV2.x, 1f, randV2.y);

        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
    }

    public void NotifyPCCaptured(PCBehavior behavior)
    {
        if (behavior == PCTarget)
        {
            ChoosePCTarget();
        }
    }

    public void ChoosePCTarget()
    {
        PCTarget = PCManager.Instance.GetClosestNextPC(trans.position, selfTeam);

        Vector2 randV2 = Random.insideUnitCircle * PCTarget.PCRange;

        // We set target  to some place around the pc (but still inside)
        agent.SetDestination(PCTarget.trans.position + new Vector3(randV2.x, 1f, randV2.y));
    }

    public void TakeDamage(float amount, string bulletowner)
    {
        healthpoint -= amount;

        if (healthpoint < 0)
        {
            Spawner.Instance.NotifyDeath(this);
            PointsManager.Instance.AddKillPoints(bulletowner);
        }
    }
}
