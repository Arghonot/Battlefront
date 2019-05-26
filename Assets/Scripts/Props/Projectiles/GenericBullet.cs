using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is meant to be the generic version of a bullet.
/// it is meant to be used with the assault rifles for exemple.
/// </summary>
public class GenericBullet : GenericProjectile
{
    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
            //            gameObject.SetActive(false);
        }
    }

    // TODO don't use getcomponent but a dictionary of instanceID and player on spawner
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Red" || collision.gameObject.tag == "Blue"))
        {
            if ((collision.gameObject.tag != gameObject.tag) || (collision.gameObject.tag == gameObject.tag && GameManager.Instance.IsTeamKillAllowed))
            {
                collision.gameObject.GetComponent<PlayerAI>().TakeDamage(damage, owner);
            }
        }

        Destroy(gameObject);
    }
}
