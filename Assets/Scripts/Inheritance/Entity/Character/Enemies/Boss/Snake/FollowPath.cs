using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

namespace Team17.BallDash
{
    public class FollowPath : Entity
    {
        public PathCreator pathCreator;
        public float speed = 5f;
        public bool startPathFollowing;

        private float distanceTravelled;
        private Vector3 initialPos;
        private Quaternion initialRot;

        private void Awake()
        {
            initialPos = transform.localPosition;
            initialRot = transform.localRotation;
            ResetTransform();
        }

        public void Update()
        {
            MoveAlongSpline();
        }

        public void MoveAlongSpline()
        {
            if (startPathFollowing)
            {
                pathCreator.path.ended = false;
                distanceTravelled += speed * Time.fixedDeltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.StopAndTrigger);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.StopAndTrigger);
            }

        }

        public void EndSplineResetValues()
        {
            ResetTransform();
            startPathFollowing = false;
            distanceTravelled = 0f;
        }

        public void ResetTransform()
        {
            transform.localPosition = initialPos;
            transform.localRotation = initialRot;
        }

    }
}
