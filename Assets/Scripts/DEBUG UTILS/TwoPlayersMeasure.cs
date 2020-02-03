using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPlayersMeasure : MonoBehaviour
{
    public int redPlayerNumber;
    public int BluePlayerNumber;

    public float Distance;

    // Update is called once per frame
    void Update()
    {
        if (redPlayerNumber != 0 && BluePlayerNumber != 0)
        {
            Distance = Vector3.Distance(
                Spawner.Instance.GetBluePosition(BluePlayerNumber),
                Spawner.Instance.GetRedPosition(redPlayerNumber));
        }
    }
}
