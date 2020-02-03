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

[System.Serializable]
public class DeadPlayerSlot
{
    public PlayerAI player;
    public float TimeSinceDeath;
}

public class Spawner : MonoBehaviour
{
    //public GameObject BluePrefab;
    //public GameObject RedPrefab;
    public Transform PlayerContainer;
    public Transform DeadStorage;
    public List<List<PlayerAI>> Teams;
//    public List<List<PlayerAI>> TeamsDeads;
    public List<DeadPlayerSlot> DeadPlayers;

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
        if (GameManager.Instance.IsGameRunning)
        {
            UpdateDeadPlayersTimeCount();
        }
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
        DeadPlayers = new List<DeadPlayerSlot>();

        Teams.Add(new List<PlayerAI>());
        Teams.Add(new List<PlayerAI>());

        CreateTeam(Team.Blue);
        CreateTeam(Team.Red);
    }

    void CreateTeam(Team team)
    {
        for (int i = 0; i < GameManager.Instance.PlayerPerTeam; i++)
        {
            var SoldierClass = (SoldierType)UnityEngine.Random.Range(
                     0,
                     Convert.ToInt32(
                         Enum.GetValues(typeof(SoldierType)).Cast<SoldierType>().Max()));

            //var SoldierClass = SoldierType.RocketLauncher;

            Teams[(int)team].Add(
                Instantiate(
                        SoldierClassManager.Instance.GetPrefab(
                            team,
                            SoldierClass)).
                                GetComponent<PlayerAI>());

            //Teams[(int)team].Last().Init(team, SoldierType.Assault);

            Teams[(int)team].Last().Init(team, SoldierClass);

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
        RefillTeam();
        //ReInitPlayerInstance();
        DispatchTeams();
    }

    void ReInitPlayerInstance()
    {
        for (int i = 0; i < Teams.Count; i++)
        {
            for (int x = 0; x < Teams[i].Count; x++)
            {
                Teams[i][x].agent.enabled = true;
            }
        }
    }

    void    RefillTeam()
    {
        //// We refill the teams with the deadplayers
        //Teams[(int)Team.Blue].AddRange(TeamsDeads[(int)Team.Blue]);
        //Teams[(int)Team.Red].AddRange(TeamsDeads[(int)Team.Red]);
        
        // We refill the teams with the deadplayers
        Teams[(int)Team.Blue].AddRange(DeadPlayers.Where(x => x.player.selfTeam == Team.Blue).Select(x => x.player));
        Teams[(int)Team.Red].AddRange(DeadPlayers.Where(x => x.player.selfTeam == Team.Red).Select(x => x.player));
        DeadPlayers = new List<DeadPlayerSlot>();
    }

    void ResetTeamsTickets()
    {
        for (int i = 0; i < tickets.Count; i++)
        {
            tickets[i] = GameManager.Instance.ticketsPerTeam;
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

    // Should only be used to restart a round / game
    void DispatchTeam(Team team)
    {
        PCBehavior mainPC;


        for (int i = 0; i < GameManager.Instance.PlayerPerTeam; i++)
        {
            mainPC = PCManager.Instance.GetRandomPC(team);

            if (mainPC == null)
            {
                Debug.LogError("No PC was found for the " + team + " team");
                return;
            }

            Teams[(int)team][i].DispatchPlayer(mainPC);
        }
        //PCBehavior mainPC = PCManager.Instance.GetRandomPC(team);

        //if (mainPC == null)
        //{
        //    Debug.LogError("No PC was found for the " + team + " team");
        //    return;
        //}

        //for (int i = 0; i < GameManager.Instance.PlayerPerTeam; i++)
        //{
        //    Teams[(int)team][i].DispatchPlayer(mainPC);
        //}
    }

    #endregion

    #region RUNTIME FUNCTIONS

    /// <summary>
    /// This function is to be called at each frame in order to
    /// keep a good track at their death time.
    /// It also reassign a deadplayer when it is time.
    /// </summary>
    void UpdateDeadPlayersTimeCount()
    {
        for (int i = 0; i < DeadPlayers.Count; i++)
        {
            DeadPlayers[i].TimeSinceDeath += Time.deltaTime;

            if (DeadPlayers[i].TimeSinceDeath > GameManager.Instance.TimeBeforeRespawn)
            {
                ReassingSinglePlayer(DeadPlayers[i].player);
            }
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

        player.gameObject.SetActive(true);

        //player.SetNewPosition(PCManager.Instance.GetRandomPC(player.selfTeam).trans.position);
        player.DispatchPlayer(spawnPC);
        //tickets[(int)player.selfTeam] -= 1;
        Teams[(int)player.selfTeam].Add(player);
        DeadPlayers.Remove(DeadPlayers.Where(x => x.player == player).First());
        //TeamsDeads[(int)player.selfTeam].Remove(player);
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
    }

    public List<Transform>  getEnemiesInRange(Team team, Vector3 position, float requestRange)
    {
        return Teams[team == Team.Blue ? 1 : 0].
            Select(x => x.transform).
            Where(x => Vector3.Distance(
                x.transform.position, position) < requestRange).
            ToList();
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
        DeadPlayers.Add(new DeadPlayerSlot()
        {
            player = deadplayer,
            TimeSinceDeath = 0f
        });
//        TeamsDeads[(int)deadplayer.selfTeam].Add(deadplayer);
        Teams[(int)deadplayer.selfTeam].Remove(deadplayer);

        //StartCoroutine(deadplayer.WaitForRespawn());

        tickets[(int)deadplayer.selfTeam] -= 1;

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

    public bool isAlive(Transform player)
    {
        for (int i = 0; i < DeadPlayers.Count; i++)
        {
            if (DeadPlayers[i].player.name == player.name)
                return false;
        }

        return true;
    }

    #endregion

    #region LEGACY CODE

    //// NOTE for now the spawner get to choose the spawn pc, perhaps this should change ?
    //void ReassignPlayerFromTeam(Team team)
    //{
    //    if (tickets[(int)team] > 0)
    //    {
    //        if (TeamsDeads[(int)team].Count > 0)
    //        {
    //            TeamsDeads[(int)team][0].SetNewPosition(TeamsDeads[(int)team][0].PCTarget.trans.position);
    //            TeamsDeads[(int)team][0].gameObject.SetActive(true);
    //            TeamsDeads[(int)team][0].DispatchPlayer(PCManager.Instance.GetRandomPC(team));
    //            Teams[(int)team].Add(TeamsDeads[(int)team][0]);
    //            TeamsDeads[(int)team].RemoveAt(0);
    //        }
    //    }
    //}

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

    #endregion

    #region DEBUG

    public Vector3 GetRedPosition(int index)
    {
        // in case none was found
        if (index < 0 || index > Teams[1].Count)
        {
            return Vector3.zero;
        }
        return Teams[1][index].transform.position;
    }

    public Vector3 GetBluePosition(int index)
    {
        // in case none was found
        if (index < 0 || index > Teams[0].Count)
        {
            return Vector3.zero;
        }

        return Teams[0][index].transform.position;
    }

    #endregion

    #region LEGACY

    #endregion
}
