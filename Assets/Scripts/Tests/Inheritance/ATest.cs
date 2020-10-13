using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour
{
    protected void LogName()
    {
        Debug.Log(GetType().ToString());
    }
}

[ExecuteInEditMode]
public class ATest : B
{
    public bool run;

    void Update()
    {
        if (run)
        {
            run = false;
            LogName();
        }
    }
}
