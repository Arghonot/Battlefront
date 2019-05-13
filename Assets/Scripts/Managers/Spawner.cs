using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Blue,
    Red,
    Yellow,
    None
}

public class Spawner : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform PlayerContainer;
    public int PlayerPerTeam;
    public int ticketsPerTeam;
    public Transform DeadStorage;
    public List<List<PlayerAI>> Teams;
    public List<List<PlayerAI>> TeamsDeads;
    List<int> tickets;

    private static Spawner instance = null;
    public static Spawner Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Spawner>();
            return instance;
        }
    }

    private void Start()
    {
        InitGame();
        DispatchTeams();
    }

    private void Update()
    {
        ReassignPlayer();
    }

    void SetTeamObjective(Team team)
    {
        for (int i = 0; i < Teams[(int)team].Count; i++)
        {
            Teams[(int)team][i].ChoosePCTarget();
        }
    }

    void DispatchTeams()
    {
        DispatchTeam(Team.Blue);
        DispatchTeam(Team.Red);
    }

    void DispatchTeam(Team team)
    {
        PCBehavior mainPC = PCManager.Instance.GetTeamsPC(team);

        if (mainPC == null)
        {
            Debug.LogError("No PC was found for the " + team + " team");
            return;
        }

        for (int i = 0; i < PlayerPerTeam; i++)
        {
            Teams[(int)team][i].DispatchPlayer(mainPC);
        }
    }

    void InitGame()
    {
        tickets = new List<int>();
        tickets.Add(ticketsPerTeam);
        tickets.Add(ticketsPerTeam);

        Teams = new List<List<PlayerAI>>();
        TeamsDeads = new List<List<PlayerAI>>();

        TeamsDeads.Add(new List<PlayerAI>());
        TeamsDeads.Add(new List<PlayerAI>());

        Teams.Add(new List<PlayerAI>());
        Teams.Add(new List<PlayerAI>());

        CreateTeam(Team.Blue);
        CreateTeam(Team.Red);
    }

    void CreateTeam(Team team)
    {
        for (int i = 0; i < PlayerPerTeam; i++)
        {
            Teams[(int)team].Add(Instantiate(PlayerPrefab).GetComponent<PlayerAI>());

            Teams[(int)team].Last().Init(team);

            Teams[(int)team].Last().trans.SetParent(PlayerContainer);
            Teams[(int)team].Last().gameObject.name = string.Join("_", new string[]
            {
                "Player",
                i.ToString(),
                team.ToString()
            });
            Teams[(int)team].Last().gameObject.tag = team.ToString();
            Teams[(int)team].Last().gameObject.layer = LayerMask.NameToLayer(team.ToString());
            PointsManager.Instance.RegisterPlayerForScore(Teams[(int)team].Last().gameObject.name);
        }
    }

    void ReassignPlayer()
    {
        if (GameManager.Instance.IsGameRunning)
        {
            ReassignPlayerFromTeam(Team.Blue);
            ReassignPlayerFromTeam(Team.Red);

            CheckIfAnyTeamRanOutOfTickets();
        }
    }

    void CheckIfAnyTeamRanOutOfTickets()
    {
        if (tickets[(int)Team.Blue] <= 0)
        {
            GameManager.Instance.NotifyTeamEndOfTickets(Team.Red);
        }
        else if (tickets[(int)Team.Red] <= 0)
        {
            GameManager.Instance.NotifyTeamEndOfTickets(Team.Blue);
        }
    }

    void ReassignPlayerFromTeam(Team team)
    {
        if (tickets[(int)team] > 0)
        {
            if (TeamsDeads[(int)team].Count > 0)
            {                   
                TeamsDeads[(int)team][0].trans.position = TeamsDeads[(int)team][0].PCTarget.trans.position;
                TeamsDeads[(int)team][0].gameObject.SetActive(true);
                TeamsDeads[(int)team][0].DispatchPlayer(PCManager.Instance.GetRandomPC(team));
                tickets[(int)team] -= 1;
                Teams[(int)team].Add(TeamsDeads[(int)team][0]);
                TeamsDeads[(int)team].RemoveAt(0);
            }
        }
    }


    public void NotifyPcCaptured(PCBehavior behavior)
    {
        for (int i = 0; i < Teams.Count; i++)
        {
            for (int x = 0; x < Teams[i].Count; x++)
            {
                Teams[i][x].NotifyPCCaptured(behavior);
            }
        }
    }

    public int GetMaxAmountTeam()
    {
        return (int)Team.None - 1;
    }

    public Team TeamFromString(string name)
    {
        if (name == "Blue")
            return Team.Blue;
        if (name == "Red")
            return Team.Red;
        if (name == "Yellow")
            return Team.Yellow;
        return Team.None;
    }

    public Color GetColorFromTeam(Team team)
    {
        if (team == Team.Blue)
            return Color.blue;
        if (team == Team.Red)
            return Color.red;
        if (team == Team.Yellow)
            return Color.yellow;
        return Color.white;
    }

    // TODO change this to add all the position of all the players of the enemies teams
    public Vector3[] GetEnemiesPositions(Team team)
    {
        // The team argument of this function is the team of the ai that sent the request
        if (team == Team.Blue)
            team = Team.Red;
        else
            team = Team.Blue;

        Vector3[] Array = new Vector3[Teams[(int)team].Count];

        for (int i = 0; i < Teams[(int)team].Count; i++)
        {
            if (Teams[(int)team][i] != null)
            {
                Array[i] = Teams[(int)team][i].transform.position;
            }
        }

        return Array;
    }

    public Transform GetPlayerTransformFromIndex(int index, Team team)
    {
        if (team == Team.Blue)
            team = Team.Red;
        else
            team = Team.Blue;

        if (index > Teams[(int)team].Count || index < 0)
            return null;

        return Teams[(int)team][index].trans;
    }

    public void NotifyDeath(PlayerAI deadplayer)
    {
        deadplayer.gameObject.SetActive(false);
        deadplayer.trans.position = DeadStorage.position;
        TeamsDeads[(int)deadplayer.selfTeam].Add(deadplayer);
        Teams[(int)deadplayer.selfTeam].Remove(deadplayer);
    }

    public Team GetTeamWithMostTickets()
    {
        int teamtickets = -1;
        int team = -1;

        for (int i = 0; i < tickets.Count; i++)
        {
            if (tickets[i] > teamtickets)
            {
                teamtickets = tickets[i];
                team = i;
            }
        }

        return (Team)team;
    }
}
