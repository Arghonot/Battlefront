using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public GameObject BulletPrefab;
    public List<Bullet> Bullets;
    public int AmountOfBulletPreload;

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

    private void Awake()
    {
        Bullets = new List<Bullet>();

        for (int i = 0; i < AmountOfBulletPreload; i++)
        {
            Bullets.Add(Instantiate(BulletPrefab).GetComponent<Bullet>());
            //Bullets
        }
    }
}
