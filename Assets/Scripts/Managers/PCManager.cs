using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PCManager : MonoBehaviour
{
    public List<PCBehavior> PCs;
    public int PointsToConquer;
    public LayerMask maskForPlayer;

    List<Team> TeamsAtStartup;

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

    private void Start()
    {
        TeamsAtStartup = PCs.Select(x => x.ControlledBy).ToList();

        GameManager.Instance.RegisterForInitActions(ResetPCs, -1, "[PCMANAGER] -PC manager reseted.");
    }

    public PCBehavior GetTeamsPC(Team team)
    {
        return GetRandomPC(team);
        var pcs = PCs.Where(x => x.ControlledBy == team);

        print(pcs.Count());

        return pcs.ElementAt(Random.Range(0, pcs.Count() - 1));

        //for (int i = 0; i < PCs.Count; i++)
        //{
        //    if (PCs[i].ControlledBy == team)
        //        return PCs[i];
        //}

        //return null;
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
        int index = Random.Range(0, pcs.Length);

        if (pcs.Length == 0)
        {
            // else return a neutral pc
            if (team != Team.None)
            {
                return GetFullRandomPC();
            }
            // else one of it's own team
            else
            {
                return GetRandomPC(team == Team.Blue ? Team.Red : Team.Blue);
            }
        }

        return pcs[index];
    }

    public PCBehavior GetFullRandomPC()
    {
        int index = Random.Range(0, PCs.Count);

        if (PCs.Count == 0)
        {
            return null;
        }

        return PCs[index];
    }

    public PCBehavior GetRandomNeutralEnemyPC(
        Vector3 playerposition,
        Team team)
    {
        var otherPCs = PCs.Where(x => x.ControlledBy != team);

        if (otherPCs.Count() > 0)
        {
            return otherPCs.ElementAt(Random.Range(0, otherPCs.Count()));
        }

        return GetFullRandomPC();
    }

    public PCBehavior GetFirstNeutralEnemyPC(Vector3 playerposition,
        Team team,
        bool shalldebugactions = false)
    {
        float dist = float.MaxValue;
        int index = -1;
        var SelfPCs = PCs.Where(x => x.ControlledBy == team);

        if (shalldebugactions)
        {
            print(SelfPCs.Count() + "    " + team);
        }

        for (int i = 0; i < PCs.Count; i++)
        {
            if (!SelfPCs.Contains(PCs[i]))
            {
                float currentdist = Vector3.Distance(
                    playerposition,
                    PCs[i].transform.position);
                if (currentdist < dist)
                {
                    dist = currentdist;
                    index = i;
                }
            }
        }

        if (index > -1)
        {
            return PCs[index];
        }

        return GetFullRandomPC();

        //List<PCBehavior> unconqueredPC = PCs.Where(c => c.ControlledBy != team).ToList();

        //return unconqueredPC[Random.Range(0, unconqueredPC.Count - 1)];
    }

    // Get the closest which is not from this team
    public PCBehavior GetClosestNextPC(Vector3 pos, Team team)
    {
        PCBehavior behavior = GetClosestPC(pos, team);
        PCBehavior[] availablePC = PCs.Where(x => x.ControlledBy != team).ToArray();

        if (availablePC.Length == 0)
            return null;

        return availablePC[Random.Range(0, availablePC.Length)];
    }

    /// <summary>
    /// This function return a random pc that is controlled by
    /// the said team.
    /// </summary>
    /// <returns>The pc found, null if none.</returns>
    public PCBehavior GetAlreadyControlledPC(Team team)
    {
        List<PCBehavior> PCsFound = PCs.Where(x => x.ControlledBy == team).ToList();

        if (PCsFound.Count() == 0)
            return null;

        return PCsFound[Random.Range(0, PCsFound.Count())];
    }

    void ResetPCs()
    {
        for (int i = 0; i < PCs.Count; i++)
        {
            PCs[i].ForceReinit(TeamsAtStartup[i]);
        }
    }

    public void NotifyPCChanged(PCBehavior behavior)
    {
        //Spawner.Instance.NotifyPcCaptured(behavior);
    }

    #region LEGACY


    #endregion
}
