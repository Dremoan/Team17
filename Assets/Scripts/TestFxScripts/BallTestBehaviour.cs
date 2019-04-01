using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTestBehaviour : MonoBehaviour
{

    [SerializeField] private Rigidbody body;

    private GameObject particleLaunch;
    private GameObject particleTrail;
    private GameObject particleImpact;

    private float impactLife;

    public void Launch(float speed, float launchLifetime, float impactLifeTime, GameObject launchFx, GameObject trailFx, GameObject impactFx)
    {
        particleLaunch = launchFx;
        particleTrail = trailFx;
        particleImpact = impactFx;
        impactLife = impactLifeTime;
        body.velocity = Vector3.right * speed;
        GameObject launchClone = GameObject.Instantiate(launchFx, transform.position, Quaternion.identity);
        PlayParticle(launchClone);
        Destroy(launchClone, launchLifetime);
        GameObject trailClone = GameObject.Instantiate(trailFx, transform.position, Quaternion.identity);
        trailClone.transform.parent = transform;
        PlayParticle(trailClone);
    }

    private void PlayParticle(GameObject test)
    {
        if(test.GetComponent<ParticleSystem>() != null)
        {
            test.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject impactClone = GameObject.Instantiate(particleImpact, transform.position, Quaternion.identity);
        PlayParticle(impactClone);
        Destroy(impactClone, impactLife);
        Destroy(this.gameObject);
    }


}
