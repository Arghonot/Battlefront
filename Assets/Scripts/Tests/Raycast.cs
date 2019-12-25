using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Transform target;
    public LayerMask mask;

    private void Update()
    {
        Ray ray = new Ray(
            transform.position,
            target.position - transform.position);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, mask))
        {
            print(hit.collider.name);
        }
        else
        {
            print("none");
        }
    }
}
