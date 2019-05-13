using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainWeapons
{
    AssaultRifle,
    GrenadeLauncher
}

[System.Serializable]
public class SoldierClass
{
    public string Name;

    public MainWeapons MainWeapon;
}

public class SoldierClassManager : MonoBehaviour
{
    public float SoldierHealthPoints;

    public List<SoldierClass> Classes;

    private static SoldierClassManager instance = null;
    public static SoldierClassManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SoldierClassManager>();
            return instance;
        }
    }
}
