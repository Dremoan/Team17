using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class FeedBack : MonoBehaviour
    {
        //Particles
        [SerializeField] private bool particles = false;
        [SerializeField] private ParticleSystem[] particlesSystems;
        //shake
        [SerializeField] private bool shake = false;
        [SerializeField] private bool useSpecificTransform = false;
        [SerializeField] private Transform[] specificTransformToShake;
        [SerializeField] private float shakeAmplitude = 5f;
        [SerializeField] private float shakeTime = 2f;

        private Transform[] usedTransform;
        private bool isShaking = false;
        private float shakeDecrementer;

        private void Update()
        {
            ShakeManagement();
        }

        [ContextMenu("Test feedback")]
        public void PlayFeedBack()
        {
            if(particles)
            {
                for (int i = 0; i < particlesSystems.Length; i++)
                {
                    particlesSystems[i].Play();
                }
            }
            if(shake)
            {
                if(useSpecificTransform)
                {
                    usedTransform = specificTransformToShake;
                }
                else
                {
                    usedTransform = new Transform[GameManager.state.VirtualCameraTargets.Count];
                    for (int i = 0; i < usedTransform.Length; i++)
                    {
                        usedTransform[i] = GameManager.state.VirtualCameraTargets[i].transform;
                    }
                }
                shakeDecrementer = shakeTime;
                isShaking = true;
            }
        }

        private void ShakeManagement()
        {
            if (isShaking)
            {

                if(shakeDecrementer > 0)
                {
                    shakeDecrementer -= Time.deltaTime; 
                    for (int i = 0; i < usedTransform.Length; i++)
                    {
                        Vector3 newPos = (Random.insideUnitCircle * shakeAmplitude * Mathf.InverseLerp(0, shakeTime, shakeDecrementer));
                        usedTransform[i].localPosition = new Vector3(newPos.x, newPos.y, usedTransform[i].localPosition.z);
                    }
                }
                else
                {
                    shakeDecrementer = shakeTime;
                    isShaking = false;
                }
            }
        }

        public bool Particles { get => particles; }
        public bool Shake { get => shake; }
        public bool UseSpecificTransform { get => useSpecificTransform; }
    }
}
