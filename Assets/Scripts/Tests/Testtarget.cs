using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testtarget : MonoBehaviour
{
    public float life = 100f;

    public void TakeDamage(float amount)
    {
        life -= amount;

        if (life < 0)
        {
            TestTeamManager.Instance.NotifyDeath(transform);
            print("Die");
            Destroy(gameObject);
        }
    }
}
