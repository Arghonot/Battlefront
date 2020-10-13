using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public  class WeaponInspector
{
    public WeaponType type;
    public GenericGun gunPrefab;
}

[Serializable]
public class PoolableAsset
{
    [SerializeField] public string Name;// { get; set; }
    [SerializeField] public GameObject Prefab;// { get; set; }
}

public class AssetManager : MonoBehaviour
{
    /// <summary>
    /// A dictionary storing all the assets that will be used at runtime.
    /// AssetName, all the dirty instantiated assets for this name.
    /// </summary>
    // TODO is string really faster ? should we use a stack instead ?
    Dictionary<string, List<object>> Container;
    public List<PoolableAsset> PoolableAssets;

    //public List<WeaponInspector> Guns;

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

    void Awake()
    {
        Container = new Dictionary<string, List<object>>();

        for (int i = 0; i < PoolableAssets.Count; i++)
        {
            Container.Add(PoolableAssets[i].Name, new List<object>());
        }
    }

    //private void Update()
    //{
    //    foreach (var item in Container)
    //    {
    //        Debug.Log(item.Key + "  " + item.Value.Count());
    //    }
    //}

    public T Get<T>(string name)
    {
        // if we don't have any of these anymore, we return an instantiated version
        if (Container[name].Count() == 0)
        {
            return Instantiate(
                PoolableAssets.
                    Where(x => x.Name == name).
                    First().Prefab).GetComponent<T>();
        }
        // we take the first in the row
        var Asset = Container[name].First();

        Container[name].Remove(Asset);

        return (T)Asset;
    }

    /// <summary>
    /// Store an object in the pool.
    /// The object should be deactivated prior to this stage.
    /// </summary>
    /// <param name="typename">The type of the object.</param>
    /// <param name="obj">The object to be stored.</param>
    public void Add(string typename, object obj)
    {
        Container[typename].Add(obj);
    }

    //public GenericGun getGunForClass(WeaponType weapontype)
    //{
    //    return Guns.Where(x => x.type == weapontype).First().gunPrefab;
    //}
}
