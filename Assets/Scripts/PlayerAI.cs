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
}

public class PlayerAI : MonoBehaviour
{
    public Transform target;

    public SoldierType OwnClass;
    public Transform trans;
    public Team selfTeam;
    public bool IsAlive;
    public GenericGun gun;

    float healthpoint = 100f;
    public float ViewCone;
    public float AimCone;
    public float GunRange;
    public float FollowDistance;
    public NavMeshAgent agent;
    Rigidbody body;

    float TimeSinceLerp;
    public Transform spine;
    public Vector3 Offset;
    Quaternion initialRotation;
    Animator _anim;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    BT.GenericDictionary gd = new BT.GenericDictionary();

    public DebugArguments DebugArgs;

    #region UNITY API

    private void Start()
    {
        var offset = spine.eulerAngles - Offset;

        initialRotation = spine.rotation;
    }

    private void Update()
    {
        if (gd.Get<Transform>("Target") != null)
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

    private void LateUpdate()
    {
        if (gd.Get<Transform>("Target") == null)
        {
            TimeSinceLerp = 0f;
            //SpineRotation(Quaternion.LookRotation(Vector3.zero));
            return;
        }

        TimeSinceLerp += Time.deltaTime;
        var lookRotation = Quaternion.LookRotation(target.position - spine.position);

        SpineRotation(lookRotation);
    }

    void SpineRotation(Quaternion lookRotation)
    {
        // We do want to have a smooth spine rotation
        lookRotation = Quaternion.Lerp(
            spine.rotation,
            lookRotation,
            TimeSinceLerp);

        spine.rotation = lookRotation * Quaternion.Euler(
            (initialRotation.eulerAngles - Offset).x,
            (initialRotation.eulerAngles - Offset).y,
            (initialRotation.eulerAngles - Offset).z);
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
        body = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        // setup it's own color
        //GetComponent<MeshRenderer>().material.color = team == Team.Blue ? Color.blue : team == Team.Red ? Color.red : Color.yellow;
        body.isKinematic = true;

        OwnClass = specialtie;
        selfTeam = team;
        trans = transform;
        IsAlive = true;

        SetGDValues();

        BTExecutor.Instance.RegisterContext(gd);
    }

    void SetGDValues()
    {
        gd.Set<bool>("Alive", true);
        gd.Set<NavMeshAgent>("agent", agent);
        gd.Set<Team>("SelfTeam", selfTeam);
        gd.Set<Transform>("self", transform);
        gd.Set<float>("GunRange", GunRange);
        gd.Set<float>("FollowDistance", FollowDistance);
        gd.Set<float>("VisionAngle", ViewCone);

        /// Class values
        gd.Set<float>("VisionAngle", SoldierClassManager.Instance.GetRightVisionAngle(OwnClass));
        gd.Set<float>("AimCone", 0.99f);
        gd.Set<float>("VisionDistance", SoldierClassManager.Instance.GetRightVisionDistance(OwnClass));
        gd.Set<WeaponType>("WeaponType", SoldierClassManager.Instance.GetRightWeaponForClass(OwnClass));
        gd.Set<float>("Speed", SoldierClassManager.Instance.GetRightSpeed(OwnClass));
        gd.Set<float>("MaxHealthPoints", SoldierClassManager.Instance.GetRightHealth(OwnClass));
        gd.Set<GenericGun>("Gun", gun);

        ViewCone = gd.Get<float>("VisionAngle");
    }

    #endregion

    #region SPAWN

    public void DispatchPlayer(PCBehavior pc)
    {
        IsAlive = true;
        gd.Set<bool>("Alive", true);

        SetupSpawnPosition(pc);
        ResetOwnStat();
    }

    void ResetOwnStat()
    {
        healthpoint = SoldierClassManager.Instance.GetRightHealth(OwnClass);
        agent.speed = SoldierClassManager.Instance.GetRightSpeed(OwnClass);
        this.enabled = true;
        // We enable it's agent back
        body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
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

        SetNewPosition(pc.trans.position + new Vector3(randV2.x, 1f, randV2.y));
    }

    #endregion

    #region DAMAGES

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
        body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;
    }

    IEnumerator DisablePlayerMovement(float disabilityTime)
    {
        // We disable it's agent
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.enabled = false;
        body.isKinematic = false;

        yield return new WaitForSeconds(disabilityTime);

        // We enable it's agent back
        body.isKinematic = true;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.enabled = true;

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
