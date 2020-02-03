using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the base for defining the specs of each soldier classes.
/// </summary>
[CreateAssetMenu(fileName = "Soldier classes", menuName = "Soldiers/Specs", order = 1)]
public class SoldierClassEditor : ScriptableObject
{
    public SoldierType type;
    public WeaponType MainWeapon;
    public float Speed;

    public float ViewCone;
    public float VisionDistance;

    public float followDistance;

    public float HealthPoints;

    public WeaponProfile profile;
}
