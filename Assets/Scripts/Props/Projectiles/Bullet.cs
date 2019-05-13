using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
     public Transform Trans;
     public string OwnerName;

    public float lifetime;
    public Rigidbody body;
    public float damage;

    public void Init()
    {
        body = GetComponent<Rigidbody>();
        Trans = transform;
    }

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
        if ((collision.gameObject.tag == "Red" || collision.gameObject.tag == "Blue") && collision.gameObject.tag != gameObject.tag)
        {
            collision.gameObject.GetComponent<PlayerAI>().TakeDamage(damage, OwnerName);
        }

        Destroy(gameObject);
    }
}
