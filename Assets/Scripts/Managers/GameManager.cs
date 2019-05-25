using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string playerofthegame;
    public float score;
    public bool IsGameRunning;
    public bool IsTeamKillAllowed;
    public float TimeBeforeRespawn;

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

    private void Start()
    {
        IsGameRunning = true;
    }

    public void NotifyTeamEndOfTickets(Team WinnerTeam)
    {
        IsGameRunning = false;
        playerofthegame = PointsManager.Instance.GetHighScorePlayer();
        score = PointsManager.Instance.getScoreForPlayer(playerofthegame);
    }

}
