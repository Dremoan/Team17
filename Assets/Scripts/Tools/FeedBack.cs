using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class FeedBack : MonoBehaviour
    {
        [SerializeField] private bool playOnStart = false;
        [SerializeField] private bool looping = false;
        [SerializeField] private bool hardFollowingTransform;
        [SerializeField] private Transform transformToHardFollow;
        [SerializeField] private bool tpOnTransformOnPlay;
        [SerializeField] private Transform transformToTPToOnPlay;
        //Particles
        [SerializeField] private bool particles = false;
        [SerializeField] private ParticleSystem[] particlesSystems;
        //Trail
        [SerializeField] private bool trails = false;
        [SerializeField] private TrailRenderer[] trailRenderers;
        //shake
        [SerializeField] private bool shake = false;
        [SerializeField] private bool useSpecificTransform = false;
        [SerializeField] private Transform[] specificTransformToShake;
        [SerializeField] private float shakeAmplitude = 0.2f;
        [SerializeField] private float shakeTime = 0.1f;
        //zoom
        [SerializeField] private bool zoom = false;
        [SerializeField] private AnimationCurve zoomInCurve;
        [SerializeField] private AnimationCurve zoomOutCurve;
        [SerializeField] private float speed = 0.1f;
        //rumble
        [SerializeField] private bool rumble = false;
        [SerializeField] private long rumbleTime = 5;


        private Transform[] usedTransform;
        private bool isShaking = false;
        private bool isRumbling = false;
        private bool isZooming = false;
        private float shakeDecrementer;

        private void Start()
        {
            if (playOnStart) Play();
            else Stop();
        }

        private void Update()
        {
            ShakeManagement();
            PositionManagement();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            if(tpOnTransformOnPlay)
            {
                transform.position = transformToTPToOnPlay.position;
            }

            if(particles)
            {
                for (int i = 0; i < particlesSystems.Length; i++)
                {
                    particlesSystems[i].Play();
                }
            }

            if(trails)
            {
                for (int i = 0; i < trailRenderers.Length; i++)
                {
                    trailRenderers[i].enabled = true;
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
                    usedTransform = new Transform[GameManager.state.VirtualCameraShakeTargets.Count];
                    for (int i = 0; i < usedTransform.Length; i++)
                    {
                        usedTransform[i] = GameManager.state.VirtualCameraShakeTargets[i].transform;
                    }
                }
                shakeDecrementer = shakeTime;
                isShaking = true;
            }

            if(rumble)
            {
                if(looping)
                {
                    isRumbling = true;
                }
                else
                {
                    Vibration.Vibrate(rumbleTime);
                }
            }
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            if(looping)
            {
                if (particles)
                {
                    for (int i = 0; i < particlesSystems.Length; i++)
                    {
                        particlesSystems[i].Stop();
                    }
                }
                if (trails)
                {
                    for (int i = 0; i < trailRenderers.Length; i++)
                    {
                        trailRenderers[i].enabled = false;
                    }
                }
                if (shake)
                {
                    shakeDecrementer = shakeTime;
                    isShaking = false;
                }
                if(rumble)
                {
                    isRumbling = false;
                }
            }
        }

        private void ShakeManagement()
        {
            if (isShaking)
            {
                if(shakeDecrementer > 0)
                {
                    if(!looping) shakeDecrementer -= Time.deltaTime; 
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
        
        private void VibrationManager()
        {
            if(isRumbling && looping)
            {
                Vibration.Vibrate(50);
            }
        }

        private void PositionManagement()
        {
            if(hardFollowingTransform)
            {
                transform.position = transformToHardFollow.position;
            }
        }

        public bool Particles { get => particles; }
        public bool Trails { get => trails; }
        public bool Shake { get => shake; }
        public bool UseSpecificTransform { get => useSpecificTransform; }
        public bool HardFollowingTransform { get => hardFollowingTransform; }
        public bool TpOnTransformOnPlay { get => tpOnTransformOnPlay; }
        public ParticleSystem[] ParticlesSystems { get => particlesSystems; set => particlesSystems = value; }
        public TrailRenderer[] TrailRenderers { get => trailRenderers; set => trailRenderers = value; }
        public bool Rumble { get => rumble; }
    }
}
