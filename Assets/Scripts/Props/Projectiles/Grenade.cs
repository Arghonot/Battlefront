using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : GenericProjectile
{
    public float TimeToExplode;
    public float ExplosionRadius;
    public float ExplosionForce;
    public float ExplosionDamage;
    public float DisabilityTime;

    public GameObject explosion;
    public LayerMask mask;

    private void Update()
    {
        TimeToExplode -= Time.deltaTime;

        if (TimeToExplode <= 0)
        {
            Explode();
        }
    }

    // TODO get a better way to check tag more generic
    void Explode()
    {
        float distance = 0;
        Collider[] colliders = Physics.OverlapSphere(trans.position, ExplosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].tag == "Blue" || colliders[i].tag == "Red")
            {
                // we calculate the distance
                distance = Vector3.Distance(
                            trans.position,
                            colliders[i].transform.position);

                // we set the damages
                if (!colliders[i].name.Contains("Grenade") && !colliders[i].name.Contains("Bullet"))
                {
                    if ((colliders[i].gameObject.tag != gameObject.tag) || (colliders[i].gameObject.tag == gameObject.tag && GameManager.Instance.IsTeamKillAllowed))
                    {
                        colliders[i].gameObject.GetComponent<PlayerAI>().TakeExplosiveDamage(
                        Mathf.Lerp(
                            ExplosionDamage,
                            ExplosionDamage / 4,
                            distance),
                        owner,
                        DisabilityTime);
                    }
                }
                Vector3 direction = (colliders[i].transform.position - trans.position) *
                    Mathf.Lerp(
                        0,
                        ExplosionForce,
                        distance);

                // we set it's explosion effect
                //colliders[i].GetComponent<Rigidbody>().AddForce(
                //    new Vector3(direction.x, 0, direction.z),
                //    ForceMode.Impulse);
                colliders[i].GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
            }
        }

        // Now that we dealt all the damage and explosions forces we could we delete the instance
        var explosion = Instantiate(this.explosion).transform;

        explosion.position = trans.position;
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
