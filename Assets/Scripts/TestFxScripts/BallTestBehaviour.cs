using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTestBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody body;

    private GameObject[] particleLaunch;
    private GameObject[] particleTrail;
    private GameObject[] particleImpact;

    private float impactLife;

    public void Launch(float speed, float launchLifetime, float impactLifeTime, GameObject[] launchFx, GameObject[] trailFx, GameObject[] impactFx)
    {
        particleLaunch = launchFx;
        particleTrail = trailFx;
        particleImpact = impactFx;
        impactLife = impactLifeTime;
        body.velocity = Vector3.right * speed;

        for (int i = 0; i < particleLaunch.Length; i++)
        {
            GameObject clone = GameObject.Instantiate(particleLaunch[i], transform.position, Quaternion.identity);
            PlayParticle(particleLaunch[i]);
            Destroy(particleLaunch[i], launchLifetime);
        }

        for (int i = 0; i < particleTrail.Length; i++)
        {
            GameObject clone = GameObject.Instantiate(particleTrail[i], transform.position, Quaternion.identity);
            clone.transform.parent = transform;
            PlayParticle(particleTrail[i]);
        }
    }

    private void PlayParticle(GameObject test)
    {
        if(test.GetComponent<ParticleSystem>() != null)
        {
            test.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Debug.LogWarning("no partcile system found on " + test.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < particleImpact.Length; i++)
        {
            GameObject clone = GameObject.Instantiate(particleImpact[i], transform.position, Quaternion.identity);
            PlayParticle(particleImpact[i]);
            Destroy(clone, impactLife);
        }

        Destroy(this.gameObject);
    }
}
