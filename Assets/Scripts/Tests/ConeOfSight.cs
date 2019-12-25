using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeOfSight : MonoBehaviour
{
    public Transform target;
    public float cos;

    void Update()
    {
        Vector3 directiontotarget = target.position - transform.position;

        float seingvalue = Vector3.Dot(
            directiontotarget.normalized,
            transform.forward);

        print("[seing value]" + (seingvalue) + "[directiontotarget]" + directiontotarget.ToString());

        if (seingvalue - cos > 0)
        {
            print("TRUE");

            return;
        }

        print("FALSE");
    }
}
