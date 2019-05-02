﻿using System.Collections;
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
        [SerializeField] private bool particleRotation = false;
        [SerializeField] private ParticleSystem[] particlesSystems;
        [SerializeField] private ParticleSystem[] particleSystemsToRotate;
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
        [Tooltip("The zoom will be applied on the position of this gameobject, therefore it's often better to use this option with 'tpOnTranformOnPlay' or 'hardFollowingTranform'.")]
        [SerializeField] private bool zoom = false;
        [SerializeField] private AnimationCurve zoomInCurve;
        [SerializeField] private AnimationCurve zoomOutCurve;
        [SerializeField] private float zoomedDist = 4f;
        [SerializeField] private float zoomSpeed = 0.1f;
        //rumble
        [SerializeField] private bool rumble = false;
        [SerializeField] private long rumbleTime = 5;


        private Transform[] usedShakeTransform;
        private Vector3[] initialZoomTargetsPos;
        private bool isShaking = false;
        private bool isRumbling = false;
        private bool isZoomingIn = false;
        private bool isZoomingOut = false;
        private float shakeDecrementer;
        private float zoomIncrementer;

        private void Start()
        {
            if (playOnStart) Play();
            else Stop();
        }

        private void Update()
        {
            ShakeManagement();
            ZoomManagement();
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
                    usedShakeTransform = specificTransformToShake;
                }
                else
                {
                    usedShakeTransform = new Transform[GameManager.state.VirtualCameraShakeTargets.Count];
                    for (int i = 0; i < usedShakeTransform.Length; i++)
                    {
                        usedShakeTransform[i] = GameManager.state.VirtualCameraShakeTargets[i].transform;
                    }
                }
                shakeDecrementer = shakeTime;
                isShaking = true;
            }

            if(zoom)
            {
                initialZoomTargetsPos = new Vector3[GameManager.state.VirtualCameraZoomTargets.Count];
                for (int i = 0; i < initialZoomTargetsPos.Length; i++)
                {
                    initialZoomTargetsPos[i] = GameManager.state.VirtualCameraZoomTargets[i].transform.position;
                }
                isZoomingIn = true;
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
                if(zoom)
                {
                    isZoomingOut = true;
                    isZoomingIn = false;
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
                    for (int i = 0; i < usedShakeTransform.Length; i++)
                    {
                        Vector3 newPos = (Random.insideUnitCircle * shakeAmplitude * Mathf.InverseLerp(0, shakeTime, shakeDecrementer));
                        usedShakeTransform[i].localPosition = new Vector3(newPos.x, newPos.y, usedShakeTransform[i].localPosition.z);
                    }
                }
                else
                {
                    shakeDecrementer = shakeTime;
                    isShaking = false;
                }
            }
        }

        private void ZoomManagement()
        {
            if(isZoomingIn)
            {
                if(zoomIncrementer < 1f)
                {
                    zoomIncrementer += Time.deltaTime * zoomSpeed;
                    float newZ = Mathf.Lerp(0, zoomedDist, zoomInCurve.Evaluate(zoomIncrementer));
                    for (int i = 0; i < GameManager.state.VirtualCameraZoomTargets.Count; i++)
                    {
                        float newX = Mathf.Lerp(initialZoomTargetsPos[i].x, transform.position.x, zoomInCurve.Evaluate(zoomIncrementer));
                        float newY = Mathf.Lerp(initialZoomTargetsPos[i].y, transform.position.y, zoomInCurve.Evaluate(zoomIncrementer));
                        GameManager.state.VirtualCameraZoomTargets[i].transform.position = new Vector3(newX, newY, newZ);
                    }
                }
                else
                {
                    if(!looping)
                    {
                        zoomIncrementer = 0f;
                        isZoomingOut = true;
                        isZoomingIn = false;
                    }
                    else
                    {
                        isZoomingIn = false;
                        zoomIncrementer = 0f;
                    }
                }
            }
            else if(isZoomingOut)
            {
                if (zoomIncrementer < 1)
                {
                    zoomIncrementer += Time.deltaTime * zoomSpeed;
                    float newZ = Mathf.Lerp(0, zoomedDist, zoomOutCurve.Evaluate(zoomIncrementer));
                    for (int i = 0; i < GameManager.state.VirtualCameraZoomTargets.Count; i++)
                    {
                        float newX = Mathf.Lerp(initialZoomTargetsPos[i].x, transform.position.x, zoomOutCurve.Evaluate(zoomIncrementer));
                        float newY = Mathf.Lerp(initialZoomTargetsPos[i].y, transform.position.y, zoomOutCurve.Evaluate(zoomIncrementer));
                        GameManager.state.VirtualCameraZoomTargets[i].transform.position = new Vector3(newX, newY, newZ);
                    }
                }
                else
                {
                    zoomIncrementer = 0f;
                    isZoomingOut = false;
                    isZoomingIn = false;
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

        public void RotateShapeEmitter(float newRotX)
        {
            for (int i = 0; i < particleSystemsToRotate.Length ; i++)
            {
                var shapeModule = particleSystemsToRotate[i].shape;
                shapeModule.rotation = new Vector3(newRotX,-90f,0);
            }
        }

        public void Rotate3DStartRotationZ(float newRotZ)
        {
            for (int i = 0; i < particleSystemsToRotate.Length; i++)
            {
                ParticleSystem.MainModule module = particleSystemsToRotate[i].main;
                module.startRotationZ = newRotZ * Mathf.Deg2Rad;
            }
        }

        public void Rotate3DStartRotationX(float newRotX)
        {
            for (int i = 0; i < particleSystemsToRotate.Length; i++)
            {
                ParticleSystem.MainModule module = particleSystemsToRotate[i].main;
                module.startRotationX = newRotX * Mathf.Deg2Rad;
            }
        }

        public bool Particles { get => particles; }
        public bool ParticleRotation { get => particleRotation; }
        public bool Trails { get => trails; }
        public bool Shake { get => shake; }
        public bool UseSpecificTransform { get => useSpecificTransform; }
        public bool HardFollowingTransform { get => hardFollowingTransform; }
        public bool TpOnTransformOnPlay { get => tpOnTransformOnPlay; }
        public ParticleSystem[] ParticlesSystems { get => particlesSystems; set => particlesSystems = value; }
        public ParticleSystem[] ParticleSystemsToRotate { get => particleSystemsToRotate; set => particleSystemsToRotate = value; }
        public TrailRenderer[] TrailRenderers { get => trailRenderers; set => trailRenderers = value; }
        public bool Rumble { get => rumble; }
        public bool Zoom { get => zoom; }
    }
}
