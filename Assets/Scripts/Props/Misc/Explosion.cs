using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float ExplosionDuration;
    public float MaxRange;
    Transform trans;

    private void Start()
    {
        trans = transform;
        trans.localScale = Vector3.zero;

        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        float duration = 0;

        // comes bigger
        while (duration < (ExplosionDuration / 3f) * 2f)
        {
            duration += Time.deltaTime;
            trans.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * MaxRange, duration / (ExplosionDuration / 2));

            yield return new WaitForEndOfFrame();
        }

        duration = 0;

        // comes smaller
        while (trans.localScale.magnitude > 0f)
        {
            duration += Time.deltaTime;
            trans.localScale = Vector3.Lerp(Vector3.one * MaxRange, Vector3.zero, duration / (ExplosionDuration / 3));

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);

        yield return null;
    }
}
