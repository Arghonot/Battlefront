using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum AIMode
{
    CapturingEnemiesPC,
    Wandering
}

/// <summary>
/// Use this class for debug public arguments in order to avoid to make PlayerAI dirty.
/// </summary>
[System.Serializable]
public class DebugArguments
{
    public bool DebugBT;

    public bool DebugDestination;

    public bool DebugRayEnemy;
    public GameObject Enemy;

    public bool DebugMussle;
    public bool DebugCanSee;
}

public class PlayerAI : MonoBehaviour
{
    public float RemainingHealthPoints;
    public Transform target;

    public SoldierType OwnClass;
    public Transform trans;
    public Team selfTeam;
    public bool IsAlive;

    public NavMeshAgent agent;
    //Rigidbody body;

    float TimeSinceLerp;
    public Transform spine;
    Vector3 Offset = Vector3.zero;//new Vector3(0f, -45f, 25f);
    Quaternion InitialSpineRotation;
    Animator _anim;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    Graph.GenericDicionnary gd = new Graph.GenericDicionnary();

    public DebugArguments DebugArgs;

    #region UNITY API

    IEnumerator GetInitialRotation()
    {
        yield return new WaitForSeconds(2f);
        InitialSpineRotation = spine.rotation;
    }

    private void Update()
    {
        SetDebugVals();
         
        if (gd.TryGet("Target") != null)
        {
            target = gd.Get<Transform>("Target");
            DebugArgs.Enemy = target.gameObject;
        }

        gd.Set<bool>("ShallDebug", DebugArgs.DebugBT);

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters
        //_anim.SetBool("move", shouldMove);

        _anim.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
        _anim.SetFloat("VelX", velocity.x);
        _anim.SetFloat("VelY", velocity.y);
    }

    void OnAnimatorIK()
    {
        if (gd.Get<bool>("ikActive") == false)
        {
            _anim.SetLookAtWeight(0, 0, 0, 0, 0);
        }
        else
        {
            _anim.SetLookAtWeight(1, 1, 1, 1, 1);
            _anim.SetLookAtPosition(gd.Get<Transform>("Target").position);
            gd.Set<bool>("ikActive", false);
        }
    }

    void SpineRotation(Quaternion lookRotation)
    {
        // We do want to have a smooth spine rotation
        lookRotation = Quaternion.Lerp(
            spine.rotation,
            lookRotation,
            TimeSinceLerp);

        spine.rotation = lookRotation * Quaternion.Euler(
            (InitialSpineRotation.eulerAngles - Offset).x,
            (InitialSpineRotation.eulerAngles - Offset).y,
            (InitialSpineRotation.eulerAngles - Offset).z);
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }

    #endregion

    #region INIT

    /// <summary>
    /// This is the function called when the player is first instantiated.
    /// It is supposed to init all the component this player need in order to play.
    /// </summary>
    /// <param name="team"></param>
    /// <param name="specialtie"></param>
    public void Init(Team team, SoldierType specialtie)
    {
        agent = GetComponent<NavMeshAgent>();
        //body = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        // setup it's own color
        //body.isKinematic = true;

        OwnClass = specialtie;
        selfTeam = team;
        trans = transform;
        IsAlive = true;

        SetGDValues();
        InitGun();

        BTExecutor.Instance.RegisterContext(gd);
    }

    void InitGun()
    {
        gd.Get<Gun>("Gun").Initialize(SoldierClassManager.Instance.GetClassSpecs(OwnClass).profile);
    }

    void SetGDValues()
    {
        SoldierClassEditor specs = SoldierClassManager.Instance.GetClassSpecs(OwnClass);

        var ViewCone =  1f - ((specs.ViewCone / 2f) / 90f);
        var AimCone = 1f - ((specs.profile.ConeOfSight / 2f) / 90f);

        gd.Set<bool>("Alive", true);
        gd.Set<NavMeshAgent>("agent", agent);
        gd.Set<Team>("SelfTeam", selfTeam);
        gd.Set<Transform>("self", transform);
        gd.Set<float>("GunRange", specs.profile.DistanceOfSight); // use actual gun range
        gd.Set<float>("FollowDistance", specs.followDistance); // soldier's
        //gd.Set<float>("VisionAngle", specs.ViewCone); // soldier's
        gd.Set<float>("VisionAngle", ViewCone); // soldier's
        gd.Set<Vector3>("PCTarget", Vector3.zero);

        /// Class values
        //gd.Set<float>("AimCone", specs.profile.ConeOfSight); // gun's
        gd.Set<float>("AimCone", AimCone); // gun's
        gd.Set<float>("VisionDistance", specs.VisionDistance);
        gd.Set<WeaponType>("WeaponType", specs.MainWeapon);
        gd.Set<float>("Speed", specs.Speed);
        gd.Set<float>("MaxHealthPoints", specs.HealthPoints);
        gd.Set<Gun>("Gun", GetComponentInChildren<Gun>());
        gd.Set<bool>("ShallDebug", false);
        //HalfGunRange
        gd.Set<float>("HalfGunRange", specs.profile.DistanceOfSight / 3f);
    }

    #endregion

    #region SPAWN

    public void DispatchPlayer(PCBehavior pc)
    {
        IsAlive = true;
        gd.Set<bool>("Alive", true);

        SetupSpawnPosition(pc);
        ResetOwnStat();
        agent.enabled = true;
    }

    void ResetOwnStat()
    {
        RemainingHealthPoints = SoldierClassManager.Instance.GetRightHealth(OwnClass);
        agent.speed = SoldierClassManager.Instance.GetRightSpeed(OwnClass);
        this.enabled = true;
        // We enable it's agent back
        //body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;

        gd.Set<bool>("isStunned", false);
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

    void SetupSpawnPosition(PCBehavior pc)
    {
        Vector2 randV2 = Random.insideUnitCircle * pc.PCRange;

        SetNewPosition(pc.transform.position + new Vector3(randV2.x, 1f, randV2.y));
    }

    #endregion

    #region DAMAGES

    public void TakeDamage(float amount, string bulletowner)
    {
        RemainingHealthPoints -= amount;

        if (RemainingHealthPoints <= 0)
        {
            Die(bulletowner);
        }
    }

    public void TakeExplosiveDamage(float amount, string bulletowner, float disabilityTime)
    {
        RemainingHealthPoints -= amount;

        StopAllExplosionAnimation();

        if (RemainingHealthPoints <= 0)
        {
            Die(bulletowner);
        }
        else if (agent.enabled)
        {
            StartCoroutine(DisablePlayerMovement(disabilityTime));
        }
    }

    #endregion

    #region DIE

    void Die(string bulletowner)
    {
        gd.Set<bool>("Alive", false);
        IsAlive = false;
        Spawner.Instance.NotifyDeath(this);
        PointsManager.Instance.AddKillPoints(bulletowner);
    }

    void DisableSelf()
    {
        agent.enabled = false;
        this.enabled = false;
    }

    void StopAllExplosionAnimation()
    {
        StopAllCoroutines();
        // We enable it's agent back
        //body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
    }

    IEnumerator DisablePlayerMovement(float disabilityTime)
    {
        gd.Set<Transform>("Target", null);
        gd.Set<bool>("isStunned", true);
        // We disable it's agent
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.enabled = false;
        //body.isKinematic = false;

        yield return new WaitForSeconds(disabilityTime);

        // We enable it's agent back
        //body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
        gd.Set<bool>("isStunned", false);

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

    #endregion

    #region DEBUG

    void SetDebugVals()
    {
        //Debug
        gd.Set<bool>("DebugMussle", DebugArgs.DebugMussle);
        gd.Set<bool>("DebugCanSee", DebugArgs.DebugCanSee);
    }

    private void OnDrawGizmos()
    {
        if (DebugArgs.DebugDestination)
        {
            DebugDestination();
        }
        if (DebugArgs.DebugRayEnemy)
        {
            DebugRayEnemy();
        }
    }

    void DebugRayEnemy()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, DebugArgs.Enemy.transform.position);
    }

    void    DebugDestination()
    {
        Gizmos.color = selfTeam == Team.Blue ? Color.blue : Color.red;
        Gizmos.DrawSphere(gd.Get<Vector3>("pathTarget"), 1f);
    }

    #endregion
}
