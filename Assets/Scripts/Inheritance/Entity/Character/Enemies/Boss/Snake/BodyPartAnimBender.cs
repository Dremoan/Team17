using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class BodyPartAnimBender : Entity
    {
        [SerializeField] private Animator bendAnimator;
        [SerializeField] private Transform leftTip;
        [SerializeField] private Transform rightTip;
        [SerializeField] [Range(0, 1)] private float leftBend = 0.5f;
        [SerializeField] [Range(0, 1)] private float rightBend = 0.5f;
        [SerializeField] private bool showDebug = false;

        private float initialDistFromRight = 1f;
        private float initialDistFromLeft = 1f;

        protected override void Start()
        {
            base.Start();
            initialDistFromRight = Vector3.Distance(transform.position, rightTip.position);
            initialDistFromLeft = Vector3.Distance(transform.position, leftTip.position);
        }

        protected override void Update()
        {
            base.Update();
            bendAnimator.transform.localPosition = Vector3.zero;
            CalculateLeftBend();
            CalculateRightBend();
        }

        private void CalculateRightBend()
        {
            Vector3 centerTangent = transform.right * initialDistFromRight;

            float bendDist = Vector3.Distance(transform.position + centerTangent, rightTip.position);
            float upDist = Vector3.Distance(transform.position + transform.up * 0.1f, rightTip.position);
            float downDist = Vector3.Distance(transform.position - transform.up * 0.1f, rightTip.position);


            float clampedBendDist = Mathf.Clamp(bendDist, 0, 1);
            if (upDist > downDist) clampedBendDist *= -1;

            bendAnimator.SetFloat("LeftBend", Mathf.InverseLerp(-1, 1, clampedBendDist));
        }

        private void CalculateLeftBend()
        {
            Vector3 centerTangent = -transform.right * initialDistFromLeft;

            float bendDist = Vector3.Distance(transform.position + centerTangent, leftTip.position);
            float upDist = Vector3.Distance(transform.position + transform.up * 0.1f, leftTip.position);
            float downDist = Vector3.Distance(transform.position - transform.up * 0.1f, leftTip.position);

            float clampedBendDist = Mathf.Clamp(bendDist, 0, 1);
            if (upDist > downDist) clampedBendDist *= -1;

            bendAnimator.SetFloat("RightBend", Mathf.InverseLerp(-1, 1, clampedBendDist));
        }

        public float LeftBend { get => leftBend; set => leftBend = value; }
        public float RightBend { get => rightBend; set => rightBend = value; }
    }
}
