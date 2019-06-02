using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomActions
{
    /// <summary>
    /// The action that is meant to be executed.
    /// </summary>
    public Action action;
    /// <summary>
    /// The action weight, defining it's running order relative to
    /// the other actions on the list containing this instance.
    /// </summary>
    public int weight;
    /// <summary>
    /// Only for debug, contains a brief description of the action.
    /// </summary>
    public string DebugDefinition;
}

public class GameManager : MonoBehaviour
{
    public bool IsGameRunning;
    public bool IsTeamKillAllowed;
    public float TimeBeforeRespawn;
    public int PlayerPerTeam;
    public int ticketsPerTeam;
    public int AmountOfRounds;
    public float TimeBetweenRounds;

    int RemainingRounds;
    bool isRoundOver;

    /// <summary>
    /// All the functions that needs to be called in 
    /// order to start a round shall be stored here.
    /// </summary>
    List<CustomActions> RoundInitActions;

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    #region UNITY API

    private void Awake()
    {
        RoundInitActions = new List<CustomActions>();
    }

    private void Start()
    {
        isRoundOver = false;
        RemainingRounds = AmountOfRounds;
        IsGameRunning = true;
        RegisterForInitActions(ReinitForRound);
    }

    #endregion

    #region ACTIONS MANAGEMENT

    /// <summary>
    /// This function allows to register 
    /// any function that have to be called whenever a round is started
    /// </summary>
    /// <param name="action">The action to be called.</param>
    public void RegisterForInitActions(Action newaction, int newweight = 0, string def = "")
    {
        RoundInitActions.Add(new CustomActions()
        {
            action = newaction,
            weight = newweight,
            DebugDefinition = def
        });

        RoundInitActions = RoundInitActions.OrderBy(x => x.weight).ToList();
    }

    #endregion

    void ReinitForRound()
    {
        IsGameRunning = true;
    }

    IEnumerator WaitForNewRound()
    {
        isRoundOver = true;
        yield return new WaitForSeconds(TimeBetweenRounds);

        for (int i = 0; i < RoundInitActions.Count; i++)
        {
            RoundInitActions[i].action();

            if (RoundInitActions[i].DebugDefinition != "")
            {
                print(RoundInitActions[i].DebugDefinition);
            }
        }

        isRoundOver = false;
        IsGameRunning = true;

        yield return null;
    }

    public void NotifyTeamEndOfTickets(Team WinnerTeam)
    {
        IsGameRunning = false;
        if (RemainingRounds < 0)
            return;
        
        if (RemainingRounds > 0 && (!IsGameRunning && !isRoundOver))
        {
            StartCoroutine(WaitForNewRound());
            RemainingRounds--;
        }
    }

}
