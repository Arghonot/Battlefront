using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    AssaultRifle,
    GrenadeLauncher,
    SniperRifle,
    Shotgun,
    RocketLauncher
}

public enum SoldierType
{
    Assault,
    Grenadier,
    Sniper,
    Melee,
    RocketLauncher,
    Other
}

[System.Serializable]
public class SoldierClass
{
    public string Name;
    /// <summary>
    /// This will define all this class features.
    /// </summary>
    public SoldierClassEditor specs;
    public GameObject Prefab;
}

public class SoldierClassManager : MonoBehaviour
{
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
    /// Return the weapon the class must have.
    /// </summary>
    /// <param name="soldierclass">The requested class.</param>
    /// <returns>The main weapon that this class should be equiped with.</returns>
    public WeaponType   GetRightWeaponForClass(SoldierType soldierclass)
    {
        return Classes.Where(x => x.specs.type == soldierclass).First().specs.MainWeapon;
    }

    /// <summary>
    /// Return the speed the class must have.
    /// </summary>
    /// <param name="soldierclass">The requested class.</param>
    /// <returns>The speed that this class should be equiped with.</returns>
    public float GetRightSpeed(SoldierType soldierclass)
    {
        return Classes.Where(x => x.specs.type == soldierclass).First().specs.Speed;
    }

    /// <summary>
    /// Return the amount of health the class must have.
    /// </summary>
    /// <param name="soldierclass">The requested class.</param>
    /// <returns>The amount of health that this class should be equiped with.</returns>
    public float GetRightHealth(SoldierType soldierclass)
    {
        return Classes.Where(x => x.specs.type == soldierclass).First().specs.HealthPoints;
    }

    /// <summary>
    /// Return the angle of vision the class must have.
    /// </summary>
    /// <param name="soldierclass">The requested class.</param>
    /// <returns>The vision angle that this class should be equiped with.</returns>
    public float GetRightVisionAngle(SoldierType soldierclass)
    {
        return Classes.Where(x => x.specs.type == soldierclass).First().specs.VisionAngle;
    }

    /// <summary>
    /// Return the vision distance the class must have.
    /// </summary>
    /// <param name="soldierclass">The requested class.</param>
    /// <returns>The vision distance that this class should be equiped with.</returns>
    public float GetRightVisionDistance(SoldierType soldierclass)
    {
        return Classes.Where(x => x.specs.type == soldierclass).First().specs.VisionDistance;
    }
}
