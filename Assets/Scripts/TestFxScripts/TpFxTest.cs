using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class TpFxTest : MonoBehaviour
    {
        [SerializeField] private KeyCode testKey = KeyCode.T;
        [SerializeField] private GameObject[] particleToTest;
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
            if(particleToTest != null)
            {
                for (int i = 0; i < particleToTest.Length; i++)
                {
                    GameObject clone = GameObject.Instantiate(particleToTest[i], transform.position, Quaternion.identity);
                    if(clone.GetComponent<ParticleSystem>() != null) clone.GetComponent<ParticleSystem>().Play();
                    Destroy(clone, lifeTime);
                }
            }
        }
    }


}
