using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum AIMode
{
    CapturingEnemiesPC,
    Wandering
}

public class PlayerAI : MonoBehaviour
{
    public SoldierType OwnClass;
    TargetSelector selector;
    public Transform trans;
    /// <summary>
    /// The PC the player want to capture.
    /// </summary>
    public PCBehavior PCTarget;
    /// <summary>
    /// The pc the player would go to in order to investigate for when
    /// all enemies PC are captured already.
    /// </summary>
    public PCBehavior WanderTarget;
    public AIMode mode;
    public Team selfTeam;
    public bool IsAlive;
    public Transform canon;

    float healthpoint = 100f;

    public NavMeshAgent agent;
    Rigidbody body;

    private void Update()
    {
        CheckPCRules();
    }

    /// <summary>
    /// This function check if the pc we are trying to capture is already on our team.
    /// it also check if the game should go on.
    /// </summary>
    void CheckPCRules()
    {
        // If the game is over there is no need to proceed;
        if (!GameManager.Instance.IsGameRunning)
        {
            DisableSelf();

            return;
        }

        if (mode == AIMode.CapturingEnemiesPC)
        {
            if (PCTarget == null)
            {
                SetMode(AIMode.Wandering);
                return;
            }
            if (PCTarget.ControlledBy == selfTeam)
            {
                ChoosePCTarget();
            }
        }
        else
        {
            float dist = agent.remainingDistance;

            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
            {
                //Arrived.
                ChoosePCTarget();
            }
        }
    }

    void DisableSelf()
    {
        agent.enabled = false;
        selector.enabled = false;
        this.enabled = false;
    }

    /// <summary>
    /// This is the function called when the player is first instantiated.
    /// It is supposed to init all the component this player need in order to play.
    /// </summary>
    /// <param name="team"></param>
    /// <param name="specialtie"></param>
    public void Init(Team team, SoldierType specialtie)
    {
        agent = GetComponent<NavMeshAgent>();
        selector = GetComponent<TargetSelector>();
        body = GetComponent<Rigidbody>();
        // setup it's own color
        GetComponent<MeshRenderer>().material.color = team == Team.Blue ? Color.blue : team == Team.Red ? Color.red : Color.yellow;
        body.isKinematic = true;

        if (specialtie == SoldierType.Sniper)
        {
            selector.VisionDistance *= 5f;
        }

        OwnClass = specialtie;
        selfTeam = team;
        trans = transform;
        IsAlive = true;

        selector.Init();
        // it's only the beginning of the game so we will capture.
        SetMode(AIMode.CapturingEnemiesPC);
    }

    public void SetNewPosition(Vector3 newpos)
    {
        NavMeshHit hit = new NavMeshHit();

        if (NavMesh.SamplePosition(newpos, out hit, 4f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        agent.Warp(newpos);
    }

    public void DispatchPlayer(PCBehavior pc)
    {
        IsAlive = true;

        SetupSpawnPosition(pc);
        ChoosePCTarget();
        ResetOwnStat();
    }

    void ResetOwnStat()
    {
        healthpoint = SoldierClassManager.Instance.GetRightHealth(OwnClass);
        agent.speed = SoldierClassManager.Instance.GetRightSpeed(OwnClass);
        selector.target = null;
        this.enabled = true;
        selector.enabled = true;
        // We enable it's agent back
        body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
        selector.shouldHandleEnemies = true;
    }

    void SetupSpawnPosition(PCBehavior pc)
    {
        Vector2 randV2 = Random.insideUnitCircle * pc.PCRange;

        SetNewPosition(pc.trans.position + new Vector3(randV2.x, 1f, randV2.y));
    }

    public void NotifyPCCaptured(PCBehavior behavior)
    {
        if (behavior == PCTarget && IsAlive)
        {
            ChoosePCTarget();
        }
    }

    // TODO figure out how to clear the use of StopAllExplosionAnimation
    // because we need to stop the explosion animation but also we need to get
    // a new target
    /// <summary>
    /// This function decide which enemy PC this AI want to attach.
    /// </summary>
    public void ChoosePCTarget()
    {
        PCTarget = PCManager.Instance.GetClosestNextPC(trans.position, selfTeam);
        // if it is null all the pcs have been captured.
        if (PCTarget == null)
        {
            SetMode(AIMode.Wandering);
        }
        else
        {
            // if an enemy pc has been found we want to start going there
            SetMode(AIMode.CapturingEnemiesPC);
            SetDestinationTargetForPC(PCTarget);
        }

    }

    void SetDestinationTargetForPC(PCBehavior pc)
    {

        Vector2 randV2 = Random.insideUnitCircle * pc.PCRange;

        // just in case the explosion animation is still running
        StopAllExplosionAnimation();
        gameObject.SetActive(true);

        // just another security check
        if (agent.isOnNavMesh)
        {
            // We set target  to some place around the pc (but still inside)
            agent.SetDestination(pc.trans.position + new Vector3(randV2.x, 1f, randV2.y));
        }

    }

    void SetMode(AIMode newmode)
    {
        if (newmode == AIMode.Wandering)
        {
            WanderTarget = PCManager.Instance.GetAlreadyControlledPC(selfTeam);
            SetDestinationTargetForPC(WanderTarget);
            mode = AIMode.Wandering;
        }
        else
        {
            mode = AIMode.CapturingEnemiesPC;
        }
    }

    void StopAllExplosionAnimation()
    {
        StopAllCoroutines();
        // We enable it's agent back
        body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
        selector.shouldHandleEnemies = true;
    }

    IEnumerator DisablePlayerMovement(float disabilityTime)
    {
        // We disable it's agent
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.enabled = false;
        body.isKinematic = false;
        selector.shouldHandleEnemies = false;

        yield return new WaitForSeconds(disabilityTime);

        // We enable it's agent back
        body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
        selector.shouldHandleEnemies = true;

        yield return null;
    }

    /// <summary>
    /// This function is called once this instance of player
    /// dies, it will wait a certain amount of time until it call
    /// the spawner singleton to respawn again.
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(GameManager.Instance.TimeBeforeRespawn);
        Spawner.Instance.ReassingSinglePlayer(this);
    }

    public void TakeDamage(float amount, string bulletowner)
    {
        healthpoint -= amount;

        if (healthpoint <= 0)
        {
            Die(bulletowner);
        }
    }

    public void TakeExplosiveDamage(float amount, string bulletowner, float disabilityTime)
    {
        healthpoint -= amount;

        StopAllExplosionAnimation();

        if (healthpoint <= 0)
        {
            Die(bulletowner);
        }
        else if (agent.enabled)
        {
            StartCoroutine(DisablePlayerMovement(disabilityTime));
        }
    }

    void Die(string bulletowner)
    {
        IsAlive = false;
        Spawner.Instance.NotifyDeath(this);
        PointsManager.Instance.AddKillPoints(bulletowner);
    }
}
