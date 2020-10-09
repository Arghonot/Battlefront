using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public int[] targetPoints = new int[30];

    public void Display()
    {
        foreach (var tps in targetPoints)
        {
            Debug.Log(tps);
        }
    }
}
