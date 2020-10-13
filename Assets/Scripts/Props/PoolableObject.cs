using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that can be stored in the assetManager
/// </summary>

public class PoolableObject : MonoBehaviour
{
    protected void setDirty()
    {
        gameObject.SetActive(false);
        AssetManager.Instance.Add(GetType().ToString(), this);
    }
}
