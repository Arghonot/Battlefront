using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    AssaultRifle,
    GrenadeLauncher,
    SniperRifle
}

public enum SoldierType
{
    Assault,
    Grenadier,
    Sniper
}

[System.Serializable]
public class SoldierClass
{
    public string Name;
    public SoldierType type;
    public WeaponType MainWeapon;
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

    /// <summary>
    /// Return the weapon the the class must have.
    /// </summary>
    /// <param name="soldierclass">The requested class.</param>
    /// <returns>The main weapon that this class should be equiped with.</returns>
    public WeaponType   GetRightWeaponForClass(SoldierType soldierclass)
    {
        return Classes.Where(x => x.type == soldierclass).First().MainWeapon;
    }
}
