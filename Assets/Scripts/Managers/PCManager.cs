using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PCManager : MonoBehaviour
{
    public List<PCBehavior> PCs;
    public int PointsToConquer;
    public LayerMask maskForPlayer;

    private static PCManager instance = null;
    public static PCManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PCManager>();
            return instance;
        }
    }

    public PCBehavior GetTeamsPC(Team team)
    {
        for (int i = 0; i < PCs.Count; i++)
        {
            if (PCs[i].ControlledBy == team)
                return PCs[i];
        }

        return null;
    }

    public PCBehavior    GetClosestPC(Vector3 position, Team team)
    {
        float distance = float.PositiveInfinity;
        int currentpc = -1;
        
        for (int i = 0; i < PCs.Count; i++)
        {
            // no need to test it's actual pc
            if (PCs[i].trans.position != position)
            {
                if (Vector3.Distance(PCs[i].trans.position, position) < distance)
                {
                    if (PCs[i].ControlledBy != team)
                    {
                        distance = Vector3.Distance(PCs[i].trans.position, position);
                        currentpc = i;
                    }
                }
            }
        }

        // if none was found
        if (currentpc == -1)
            return null;

        return PCs[currentpc];
    }

    public PCBehavior GetRandomPC(Team team)
    {
        PCBehavior[] pcs = PCs.Where(x => x.ControlledBy == team).ToArray();

        if (pcs.Length == 0)
            return null;

        return pcs[Random.Range(0, pcs.Length)];
    }

    PCBehavior GetFirstNeutralEnemyPC(PCBehavior behavior, Team team)
    {
        List<PCBehavior> unconqueredPC = PCs.Where(c => c.ControlledBy != team).ToList();

        return unconqueredPC[Random.Range(0, unconqueredPC.Count - 1)];
    }

    // Get the closest which is not from this team
    public PCBehavior GetClosestNextPC(Vector3 pos, Team team)
    {
        PCBehavior behavior = GetClosestPC(pos, team);
        PCBehavior[] availablePC = PCs.Where(x => x.ControlledBy != team).ToArray();

        if (availablePC.Length == 0)
            return null;
        print("toto");
        return availablePC[Random.Range(0, availablePC.Length)];
    }

    public void NotifyPCChanged(PCBehavior behavior)
    {
        Spawner.Instance.NotifyPcCaptured(behavior);
    }
}
