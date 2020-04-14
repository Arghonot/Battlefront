using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDisplayer : PointOfInterest
{
    public int Value;

    public Transform LineBegin;
    public Transform LineEnd;

    LineRenderer rend;

    private void Start()
    {
        rend = GetComponent<LineRenderer>();
        rend.enabled = false;
    }

    public override void Begin()
    {
        rend.enabled = true;
    }

    public override void End()
    {
        rend.enabled = false;
    }

    public virtual int GetValue()
    {
        return Value;
    }

    private void Update()
    {
        LineEnd.position = LineBegin.position + 
            (PointOfInterestManager.Instance.player.forward * Value);

        rend.SetPosition(0, LineBegin.position);
        rend.SetPosition(1, LineEnd.position);
    }
}
