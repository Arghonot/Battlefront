using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public Transform DeadStorage;
    public List<List<PlayerAI>> Teams;
    public List<List<PlayerAI>> TeamsDeads;
    List<int> tickets;
    Action<Team, int> TicketUpdate;

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

    #region UNITY API


    private void Start()
    {
        InitGame();
        DispatchTeams();
        GameManager.Instance.RegisterForInitActions(ResetForRound, 0, "[SPAWNER] -reset teams");
    }

    private void Update()
    {
        //ReassignPlayer();
        CheckIfAnyTeamRanOutOfTickets();

    }

    #endregion

    #region GAME INSTANCE INIT

    /// <summary>
    /// This class create the two teams and setup their players.
    /// </summary>
    void InitGame()
    {
        tickets = new List<int>();
        tickets.Add(GameManager.Instance.ticketsPerTeam);
        tickets.Add(GameManager.Instance.ticketsPerTeam);

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
        for (int i = 0; i < GameManager.Instance.PlayerPerTeam; i++)
        {
            Teams[(int)team].Add(Instantiate(PlayerPrefab).GetComponent<PlayerAI>());

            Teams[(int)team].Last().Init(team,
                (SoldierType)UnityEngine.Random.Range(
                    0,
                    Convert.ToInt32(
                        Enum.GetValues(typeof(SoldierType)).Cast<SoldierType>().Max())));

            //Teams[(int)team].Last().Init(team, SoldierType.RocketLauncher);

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

    /// <summary>
    /// This function setup the function to call whenever 
    /// the tickets cunt for a team change.
    /// </summary>
    /// <param name="newaction">The action to be executed.</param>
    public void SetupTicketUpdateAction(Action<Team, int> newaction)
    {
        TicketUpdate = newaction;
    }
    #endregion

    #region GAME ROUND INIT

    void ResetForRound()
    {
        ResetTeamsTickets();
        DispatchTeams();
    }

    void ResetTeamsTickets()
    {
        for (int i = 0; i < tickets.Count; i++)
        {
            tickets[i] = GameManager.Instance.ticketsPerTeam;
        }
    }

    // Should only be used to restart a round / game
    void DispatchTeam(Team team)
    {
        PCBehavior mainPC = PCManager.Instance.GetTeamsPC(team);

        if (mainPC == null)
        {
            Debug.LogError("No PC was found for the " + team + " team");
            return;
        }

        for (int i = 0; i < GameManager.Instance.PlayerPerTeam; i++)
        {
            Teams[(int)team][i].DispatchPlayer(mainPC);
        }
    }

    /// <summary>
    /// This function is called to dispatch all the teams that exists.
    /// </summary>
    void DispatchTeams()
    {
        DispatchTeam(Team.Blue);
        DispatchTeam(Team.Red);
    }

    #endregion

    #region RUNTIME FUNCTIONS

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

    /// <summary>
    /// This function respawn a single player after it died and waited 
    /// for a certain amount of time.
    /// </summary>
    /// <param name="player">The player we have to respawn.</param>
    public void ReassingSinglePlayer(PlayerAI player)
    {
        PCBehavior spawnPC = PCManager.Instance.GetRandomPC(player.selfTeam);

        // if no other pc are available
        if (spawnPC == null)
            return;

        //player.SetNewPosition(PCManager.Instance.GetRandomPC(player.selfTeam).trans.position);
        player.gameObject.SetActive(true);
        player.DispatchPlayer(spawnPC);
        tickets[(int)player.selfTeam] -= 1;
        Teams[(int)player.selfTeam].Add(player);
        TeamsDeads[(int)player.selfTeam].Remove(player);
    }

    /// <summary>
    /// Everytime a PC is captured, this function will be triggered.
    /// It will iterate through all the players to notify them about it.
    /// </summary>
    /// <param name="behavior">The PC that just changed team.</param>
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

    /// <summary>
    /// This function is called by AIs that want to know the position
    /// of all enemies units.
    /// </summary>
    /// <param name="team">the player own team</param>
    /// <returns>All the enemies position if any.</returns>
    public Vector3[] GetEnemiesPositions(Team team)
    {
        // The team argument of this function is the team of the ai that sent the request
        if (team == Team.Blue)
            team = Team.Red;
        else
            team = Team.Blue;

        return Teams[(int)team].Select(x => x.trans.position).ToArray();
        //Vector3[] Array = new Vector3[Teams[(int)team].Count](Teams[(int)team]);

        //for (int i = 0; i < Teams[(int)team].Count; i++)
        //{
        //    if (Teams[(int)team][i] != null)
        //    {
        //        Array[i] = Teams[(int)team][i].transform.position;
        //    }
        //}

        //return Array;
    }

    /// <summary>
    /// This function return the complete transform of a selected
    /// enemy player.
    /// </summary>
    /// <param name="index">The player's index.</param>
    /// <param name="team">The team of the selected player.</param>
    /// <returns></returns>
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
        StartCoroutine(deadplayer.WaitForRespawn());

        TicketUpdate(deadplayer.selfTeam, tickets[(int)deadplayer.selfTeam]);
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


    #endregion

    #region LEGACY CODE

    // NOTE for now the spawner get to choose the spawn pc, perhaps this should change ?
    void ReassignPlayerFromTeam(Team team)
    {
        if (tickets[(int)team] > 0)
        {
            if (TeamsDeads[(int)team].Count > 0)
            {
                TeamsDeads[(int)team][0].SetNewPosition(TeamsDeads[(int)team][0].PCTarget.trans.position);
                TeamsDeads[(int)team][0].gameObject.SetActive(true);
                TeamsDeads[(int)team][0].DispatchPlayer(PCManager.Instance.GetRandomPC(team));
                tickets[(int)team] -= 1;
                Teams[(int)team].Add(TeamsDeads[(int)team][0]);
                TeamsDeads[(int)team].RemoveAt(0);
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

    void SetTeamObjective(Team team)
    {
        for (int i = 0; i < Teams[(int)team].Count; i++)
        {
            Teams[(int)team][i].ChoosePCTarget();
        }
    }

    #endregion
}
