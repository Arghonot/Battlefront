using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : GenericProjectile
{
    public float TimeToExplode;
    public float ExplosionRadius;
    public float ExplosionForce;
    public float ExplosionDamage;

    public GameObject explosion;

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

                print(colliders[i].name + "     " + distance + "    " + Mathf.Lerp(
                        0,
                        ExplosionForce,
                        distance));

                // we set it's explosion effect
                colliders[i].GetComponent<Rigidbody>().AddForce(
                    (colliders[i].transform.position - trans.position) *
                    Mathf.Lerp(
                        0,
                        ExplosionForce,
                        distance), ForceMode.Impulse);

                // if this collider belong to somebody of the same team while TK is illegal
                // we don't apply damage on this player
                /*if (!GameManager.Instance.IsTeamKillAllowed && colliders[i].tag == team.ToString())
                {
                    break;
                }*/

                // we set the damages
                /*colliders[i].gameObject.GetComponent<PlayerAI>().TakeDamage(
                    Mathf.Lerp(
                        ExplosionDamage,
                        0,
                        distance));*/
            }
        }

        // Now that we dealt all the damage and explosions forces we could we delete the instance
        var explosion = Instantiate(this.explosion).transform;

        explosion.position = trans.position;
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
