using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

namespace Team17.BallDash
{
    public class SnakeHead : Character
    {
        public PathCreator pathCreator;
        public float speed = 5f;
        public bool startPathFollowing;
        [SerializeField] private HeadFollower[] headFollowers;

        private float distanceTravelled;
        private Vector3 initialPos;
        private Quaternion initialRot;

        protected override void Start()
        {
            base.Start();
            initialPos = transform.localPosition;
            initialRot = transform.localRotation;
            ResetTransform();
            SetFollowerInitialDistance();
        }

        protected override void Update()
        {
            base.Update();
            MoveAlongSpline();
        }

        private void SetFollowerInitialDistance()
        {
            for (int i = 0; i < headFollowers.Length; i++)
            {
                headFollowers[i].DistFromHeadAtStart = Vector3.Distance(transform.position, headFollowers[i].Transform.position);
            }
        }

        public void MoveAlongSpline()
        {
            if (startPathFollowing)
            {
                pathCreator.path.ended = false;
                distanceTravelled += speed * Time.fixedDeltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.StopAndTrigger);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.StopAndTrigger);
                for (int i = 0; i < headFollowers.Length; i++)
                {
                    float followerDist = distanceTravelled - headFollowers[i].DistFromHeadAtStart;
                    if (followerDist < 0) followerDist = 0;
                    headFollowers[i].Transform.position = pathCreator.path.GetPointAtDistance(followerDist, EndOfPathInstruction.StopAndTrigger);
                    headFollowers[i].Transform.rotation = pathCreator.path.GetRotationAtDistance(followerDist, EndOfPathInstruction.StopAndTrigger) * Quaternion.Euler(headFollowers[i].AddedRotation);
                }
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

    [System.Serializable]
    public struct HeadFollower
    {
        [SerializeField] private string name;
        [SerializeField] private Transform transform;
        [SerializeField] private Vector3 addedRotation;
        private float distFromHeadAtStart;

        public float DistFromHeadAtStart { get => distFromHeadAtStart; set => distFromHeadAtStart = value; }
        public Transform Transform { get => transform; }
        public string Name { get => name; }
        public Vector3 AddedRotation { get => addedRotation; }
    }
}
