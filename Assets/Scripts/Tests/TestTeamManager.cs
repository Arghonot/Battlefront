using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeamManager : MonoBehaviour
{
    public Transform[] Enemies;

    private static TestTeamManager instance = null;
    public static TestTeamManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TestTeamManager>();
            return instance;
        }
    }

    public Vector3[]    GetEnemiesPositions()
    {
        print("Get Enemies posiition");
        Vector3[] Array = new Vector3[Enemies.Length];

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
            {
                Array[i] = Enemies[i].position;
            }
        }

        return Array;
    }

    public Transform GetPlayerTransformFromIndex(int index)
    {
        if (index > Enemies.Length || index < 0)
            return null;

        return Enemies[index];
    }

    public void NotifyDeath(Transform deadTrans)
    {
        List<Transform> newarray = new List<Transform>(Enemies);
        newarray.Remove(deadTrans);

        Enemies = newarray.ToArray();
        /*
        Transform[] newEnemyArray = new Transform[Enemies.Length - 1];

        for (int i = 0, y = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].GetInstanceID() != deadTrans.GetInstanceID())
            {
                print(i + " " + y);
                newEnemyArray[y] = Enemies[i];
                y++;
            }
        }

        Enemies = newEnemyArray;*/
    }
}
