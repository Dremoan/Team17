using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class TpFxTest : MonoBehaviour
    {
        [SerializeField] private KeyCode testKey = KeyCode.T;
        [SerializeField] private GameObject particleToTest;
        [SerializeField] private float lifeTime;
        private bool toTheRight = false;

        private void Update()
        {
            if(Input.GetKeyDown(testKey))
            {
                if(toTheRight) // go left
                {
                    transform.position = Vector3.left * 5f;
                    PlayParticle();
                    toTheRight = false;
                }
                else // go right
                {
                    transform.position = Vector3.right * 5f;
                    PlayParticle();
                    toTheRight = true;
                }
            }
        }

        private void PlayParticle()
        {
            if(particleToTest != null && particleToTest.GetComponent<ParticleSystem>() != null)
            {
                GameObject clone = GameObject.Instantiate(particleToTest, transform.position, Quaternion.identity);
                clone.GetComponent<ParticleSystem>().Play();
                Destroy(clone, lifeTime);
            }
            else
            {
                Debug.LogWarning("no particle system found on " + gameObject.name);
            }
        }
    }


}
