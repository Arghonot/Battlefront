using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public float pointsPerKill;
    public float pointsPerCap;

    Dictionary<string, float> pointsPerPlayer;

    private static PointsManager instance = null;
    public static PointsManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PointsManager>();
            return instance;
        }
    }

    private void Awake()
    {
        pointsPerPlayer = new Dictionary<string, float>();
    }

    public void RegisterPlayerForScore(string name)
    {
        if (pointsPerPlayer == null)
            pointsPerPlayer = new Dictionary<string, float>();

        print("Register name : " + name);

        pointsPerPlayer.Add(name, 0f);
    }

    public void AddKillPoints(string name)
    {
        pointsPerPlayer[name] += pointsPerKill;
    }

    void AddCapPoints(string name)
    {
        pointsPerPlayer[name] += pointsPerCap;
    }

    public string GetHighScorePlayer()
    {
        string nameWithHigherScore = "";
        float score = -1;

        foreach (var item in pointsPerPlayer)
        {
            if (item.Value > score)
            {
                score = item.Value;
                nameWithHigherScore = item.Key;
            }
        }

        return nameWithHigherScore;
    }

    public float getScoreForPlayer(string name)
    {
        return pointsPerPlayer[name];
    }
    
    public void LogResults()
    {
        foreach (var item in pointsPerPlayer)
        {
            print(item.Key + "  " + item.Value);
        }
    }
}
