using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PCBehavior : MonoBehaviour
{
    public Transform trans;
    public Team ControlledBy;
    public float PCRange;
    public List<PCBehavior> ConnectedPC;
    public float ConquerPoints;
    public Team conqueringBy;
    public bool shouldDebugConquer;

    Light halo;

    private void Awake()
    {
        ConquerPoints = 0;
        trans = transform;
        halo = GetComponent<Light>();
        halo.color = ControlledBy == Team.Blue ? Color.blue : ControlledBy == Team.Red ? Color.red : Color.white;
    }

    private void Update()
    {
        CheckConquer();
        if (ConquerPoints > PCManager.Instance.PointsToConquer)
        {
            ConquerPoints = 0;
            halo.color = Spawner.Instance.GetColorFromTeam(conqueringBy);
            ControlledBy = conqueringBy;
            PCManager.Instance.NotifyPCChanged(this);
            conqueringBy = Team.None;

        }
    }

    void CheckConquer()
    {
        bool areAllPlayerNearbySameTeam = true;
        Collider[] colliders = Physics.OverlapSphere(trans.position, PCRange, PCManager.Instance.maskForPlayer);

        if (colliders.Length <= 0)
        {
            return;
        }

        // no need to keep adding conquering points if it's already conquered by the same guys
        if (Spawner.Instance.TeamFromString(colliders[0].tag) == ControlledBy)
        {
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.layer != colliders[0].gameObject.layer)
                areAllPlayerNearbySameTeam = false;
        }

        if (shouldDebugConquer)
            print(areAllPlayerNearbySameTeam + " " + colliders.Length);

        if (areAllPlayerNearbySameTeam)
        {
            ConquerPoints += (colliders.Length * Time.deltaTime);
            conqueringBy = Spawner.Instance.TeamFromString(colliders[0].tag);
        }
    }
}
