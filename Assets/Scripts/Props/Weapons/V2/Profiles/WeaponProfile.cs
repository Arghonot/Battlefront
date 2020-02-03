using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the base for defining the specs of each soldier classes.
/// </summary>
[CreateAssetMenu(fileName = "Gun specs", menuName = "Guns/Specs", order = 1)]
public class WeaponProfile : ScriptableObject
{
    public float DistanceOfSight;
    public float ConeOfSight;

    public float bulletVelocity;

    public float loadingTime;
}
