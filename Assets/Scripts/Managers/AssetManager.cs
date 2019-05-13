using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class WeaponInspector
{
    public WeaponType type;
    public GenericGun gunPrefab;
}

public class AssetManager : MonoBehaviour
{
    //public GameObject BulletPrefab;
    //public List<Bullet> Bullets;
    public List<WeaponInspector> Guns;
    //public int AmountOfBulletPreload;

    private static AssetManager instance = null;
    public static AssetManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AssetManager>();
            return instance;
        }
    }

    /*private void Awake()
    {
        Bullets = new List<Bullet>();

        for (int i = 0; i < AmountOfBulletPreload; i++)
        {
            Bullets.Add(Instantiate(BulletPrefab).GetComponent<Bullet>());
            //Bullets
        }
    }*/

    public GenericGun getGunForClass(WeaponType weapontype)
    {
        return Guns.Where(x => x.type == weapontype).First().gunPrefab;
    }
}
